using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : StateManager<PlayerController.PlayerState>
{
    public enum PlayerState
    {
        Idle,
        Walk,
        Interaction,
        Change
    }
    public LayerMask interactableLayer;
    public static Transform playerTransform;
    public List<Items> items = new List<Items>();
    public Animator animator;
    public Items activeItem;
    public CanvasManager canvasManager;
    private void Start()
    {
        animator = GetComponent<Animator>();
        playerTransform = transform;

        states.Add(PlayerState.Interaction, new PlayerInteractionState(PlayerState.Interaction, this));
        states.Add(PlayerState.Idle, new PlayerIdleSate(PlayerState.Idle, this));
        states.Add(PlayerState.Walk, new PlayerWalkState(PlayerState.Walk, this));
        states.Add(PlayerState.Change, new PlayerChangeState(PlayerState.Change, this));

        inputs.PlayerActions.Interactions.started += Interaction;
        inputs.PlayerActions.Movement.started += Movement;
        inputs.PlayerActions.Movement.canceled += Movement;
        inputs.PlayerActions.Change.started += ChangePlayer;

        currentState = states[PlayerState.Idle];
    }

    private void Interaction(InputAction.CallbackContext ctx)
    {
        TransitionToState(PlayerState.Interaction);
    }

    private void Movement(InputAction.CallbackContext ctx)
    {
        Vector2 movement = ctx.ReadValue<Vector2>();
        if (Mathf.Abs(movement.normalized.magnitude) > 0.1)
        {
            TransitionToState(PlayerState.Walk);
        }
        else if (movement.magnitude == 0)
        {
            TransitionToState(PlayerState.Idle);
        }
    }

    private void ChangePlayer(InputAction.CallbackContext ctx)
    {
        TransitionToState(PlayerState.Change);
    }
    private void OnEnable()
    {
        //items = InventoryManager.instance.playerInventory;
        inputs.Enable();
        canvasManager = FindObjectOfType<CanvasManager>();
        string identifier = gameObject.name; // Use character's name as identifier
        Debug.Log("Loading inventory for " + identifier); // Add debug log
        if (playerTransform != null && SaveSystem.HasSavedPosition("Player", identifier))
        {
            playerTransform.position = SaveSystem.LoadPosition("Player", identifier);
            //SaveSystem.LoadInventory("Player", identifier);
            SaveSystem.LoadInventory("Player", identifier);
        }
    }

    private void OnDisable()
    {
        string identifier = gameObject.name;
        Debug.Log("Saving inventory for " + identifier);
        SaveSystem.SavePosition(playerTransform.position, "Player", identifier);
        SaveSystem.SaveInventory(items, "Player", identifier);
        inputs.Disable();
    }
}
public class PlayerChangeState : BaseState<PlayerController.PlayerState>
{
    private PlayerController playerController;

    public PlayerChangeState(PlayerController.PlayerState state, PlayerController playerController) : base(state)
    {
        this.playerController = playerController;
    }

    public override void EnterState()
    {
        //if (playerController.canvasManager != null)
        //{
        //    playerController.canvasManager.UpdateCanvas(PlayerController.items);
        //}
        //else
        //{
        //    Debug.LogError("CanvasManager not found in the scene!");
        //}
        SceneLoader.instance.ChangeScene();
    }

    public override void ExitState()
    {
        
    }

    public override PlayerController.PlayerState GetNextState()
    {
        return PlayerController.PlayerState.Idle;
    }

    public override void UpdateState()
    {
        // Update logic if needed
    }
}

public class PlayerInteractionState : BaseState<PlayerController.PlayerState>
{
    PlayerController playerController;
    public PlayerInteractionState(PlayerController.PlayerState state, PlayerController playerController) : base(state)
    {
        this.playerController = playerController;
    }
    public override void EnterState()
    {
        Collider[] interactables = Physics.OverlapSphere(playerController.transform.position + new Vector3(0, 1, 0), 2f, playerController.interactableLayer);
        if (interactables.Length > 0)
        {
            Items item = interactables[0].GetComponent<Items>();
            item.Interacted();
            //if (!playerController.items.Contains(item))
            //{
            //    playerController.items.Add(item);
            //}
            if (!playerController.items.Contains(item))
            {
                playerController.items.Add(item);
            }
        }
        //InventoryManager.instance.UpdateInventory(playerController.items);
        
        if (playerController.canvasManager != null)
        {
            playerController.canvasManager.UpdateCanvas(playerController.items);
        }
        else
        {
            Debug.LogError("CanvasManager not found in the scene!");
        }
    }

    public override void ExitState()
    {
        
    }

    public override PlayerController.PlayerState GetNextState()
    {
        return PlayerController.PlayerState.Idle;
    }

    public override void UpdateState()
    {
        Debug.Log("Interaction State Update");
    }
}
public class PlayerIdleSate : BaseState<PlayerController.PlayerState>
{
    PlayerController playerController;
    public PlayerIdleSate(PlayerController.PlayerState state, PlayerController playerController) : base(state)
    {
        this.playerController = playerController;
    }

    public override void EnterState()
    {
        playerController.animator.SetBool("isWalking", false);
    }

    public override void UpdateState()
    {
        
    }

    public override void ExitState()
    {
        
    }

    public override PlayerController.PlayerState GetNextState()
    {
        return PlayerController.PlayerState.Walk;
    }
}
public class PlayerWalkState : BaseState<PlayerController.PlayerState>
{
    PlayerController playerController;
    public PlayerWalkState(PlayerController.PlayerState state,PlayerController playerController) : base(state)
    {
        this.playerController = playerController;
    }
    public override void EnterState()
    {
        playerController.animator.SetBool("isWalking", true);
    }

    public override void ExitState()
    {
        
    }
    public override void UpdateState()
    {
        playerController.transform.position += new Vector3(movementVector.x, 0, movementVector.y) * Time.deltaTime * 1.5f;
        playerController.transform.rotation = Quaternion.Lerp(playerController.transform.rotation, Quaternion.LookRotation(new Vector3(movementVector.x, 0, movementVector.y)), Time.deltaTime * 5f);
    }
    public override PlayerController.PlayerState GetNextState()
    {
        return PlayerController.PlayerState.Idle;
    }
}
