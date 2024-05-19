using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using static UnityEditor.Progress;

public class FordController : StateManager<FordController.FordStates>
{
    public enum FordStates
    {
        Idle,
        Walk,
        Run,
        Interaction,
        Change
    }
    public LayerMask interactableLayer;
    public static Transform fordTransform;
    public List<Items> items = new List<Items>();
    public Items activeItem;
    public CanvasManager canvasManager;

    private void Start()
    {
        fordTransform = transform;

        states.Add(FordStates.Interaction, new FordInteractionState(FordStates.Interaction, this));
        states.Add(FordStates.Idle, new FordIdleSate(FordStates.Idle, this));
        states.Add(FordStates.Walk, new FordWalkState(FordStates.Walk, this));
        states.Add(FordStates.Change, new FordChangeState(FordStates.Change, this));

        inputs.PlayerActions.Interactions.started += Interaction;
        inputs.PlayerActions.Movement.started += Movement;
        inputs.PlayerActions.Movement.canceled += Movement;
        inputs.PlayerActions.Change.started += ChangePlayer;

        currentState = states[FordStates.Idle];
    }

    public void Interaction(InputAction.CallbackContext ctx)
    {
        TransitionToState(FordStates.Interaction);
    }

    public void Movement(InputAction.CallbackContext ctx)
    {
        Vector2 movement = ctx.ReadValue<Vector2>();
        if (Mathf.Abs(movement.normalized.magnitude) > 0.1)
        {
            TransitionToState(FordStates.Walk);
        }
        else if (movement.magnitude == 0)
        {
            TransitionToState(FordStates.Idle);
        }
    }

    public void ChangePlayer(InputAction.CallbackContext ctx)
    {
        TransitionToState(FordStates.Change);
    }

    private void OnEnable()
    {
        //items = InventoryManager.instance.FordInventory;
        inputs.Enable();
        canvasManager = FindObjectOfType<CanvasManager>();
        string identifier = gameObject.name; // Use character's name as identifier
        Debug.Log("Loading inventory for " + identifier); // Add debug log
        if (fordTransform != null && SaveSystem.HasSavedPosition("Ford", identifier))
        {
            fordTransform.position = SaveSystem.LoadPosition("Ford", identifier);
            //SaveSystem.LoadInventory("Player", identifier);
            SaveSystem.LoadInventory("Ford", identifier);
        }
    }

    private void OnDisable()
    {
        string identifier = gameObject.name; // Use character's name as identifier
        Debug.Log("Saving inventory for " + identifier); // Add debug log
        SaveSystem.SavePosition(fordTransform.position, "Ford", identifier);
        SaveSystem.SaveInventory(items, "Ford", identifier);
        inputs.Disable();
    }
}

public class FordChangeState : BaseState<FordController.FordStates>
{
    private FordController fordController;
    public FordChangeState(FordController.FordStates state, FordController controller) : base(state)
    {
        fordController = controller;
    }
    public override void EnterState()
    {
        SceneLoader.instance.ChangeScene();
    }
    public override void ExitState()
    {
    }
    public override void UpdateState()
    {
    }
    public override FordController.FordStates GetNextState()
    {
        return FordController.FordStates.Idle;
    }
}
public class FordWalkState : BaseState<FordController.FordStates>
{
    private FordController fordController;
    public FordWalkState(FordController.FordStates state, FordController fordController) : base(state)
    {
        this.fordController = fordController;
    }
    public override void EnterState()
    {
        
    }
    public override void ExitState()
    {
        
    }
    public override void UpdateState()
    {
        fordController.transform.position += new Vector3(movementVector.x, 0, movementVector.y) * Time.deltaTime * 2f;
        fordController.transform.rotation = Quaternion.Lerp(fordController.transform.rotation, Quaternion.LookRotation(new Vector3(movementVector.x, 0, movementVector.y)), Time.deltaTime * 2f);
    }
    public override FordController.FordStates GetNextState()
    {
        return FordController.FordStates.Idle;
    }
}
public class FordIdleSate : BaseState<FordController.FordStates>
{
    private FordController fordController;

    public FordIdleSate(FordController.FordStates idle, FordController fordController) : base(idle)
    {
        this.fordController = fordController;
    }

    public override void EnterState()
    {
        
    }

    public override void ExitState()
    {
        
    }

    public override FordController.FordStates GetNextState()
    {
        return FordController.FordStates.Walk;
    }

    public override void UpdateState()
    {
        
    }
}

public class FordInteractionState : BaseState<FordController.FordStates>
{
    private FordController fordController;

    public FordInteractionState(FordController.FordStates interaction, FordController fordController) : base(interaction)
    {
        this.fordController = fordController;
    }

    public override void EnterState()
    {
        Collider[] interactables = Physics.OverlapSphere(fordController.transform.position + new Vector3(0, 1, 0), 2f, fordController.interactableLayer);
        if (interactables.Length > 0)
        {
            Items item = interactables[0].GetComponent<Items>();
            item.Interacted();
            //if (!fordController.items.Contains(item))
            //{
            //    fordController.items.Add(item);
            //}
            if (!fordController.items.Contains(item))
            {
                fordController.items.Add(item);
            }
        }

        //InventoryManager.instance.UpdateInventory(fordController.items);

        if (fordController.canvasManager != null)
        {
            fordController.canvasManager.UpdateCanvas(fordController.items);
        }
        else
        {
            Debug.LogError("CanvasManager not found in the scene!");
        }
    }

    public override void ExitState()
    {
        
    }

    public override FordController.FordStates GetNextState()
    {
        return FordController.FordStates.Idle;
    }

    public override void UpdateState()
    {
        
    }
}