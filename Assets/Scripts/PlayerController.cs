using Cinemachine;
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
        Change,
        Stun
    }
    public LayerMask interactableLayer;
    public List<Items> items;
    public Animator animator;
    public Items activeItem;
    public CanvasManager canvasManager;
    public static bool switchedPlayer;
    
    public CinemachineVirtualCamera virtualCamera;
    private void Start()
    {
        animator = GetComponent<Animator>();
        items = new List<Items>();

        states.Add(PlayerState.Interaction, new PlayerInteractionState(PlayerState.Interaction, this));
        states.Add(PlayerState.Idle, new PlayerIdleSate(PlayerState.Idle, this));
        states.Add(PlayerState.Walk, new PlayerWalkState(PlayerState.Walk, this));
        states.Add(PlayerState.Change, new PlayerChangeState(PlayerState.Change, this));
        states.Add(PlayerState.Stun, new PlayerStunState(PlayerState.Stun, this));

        inputs.PlayerActions.Interactions.started += Interaction;
        inputs.PlayerActions.Movement.started += Movement;
        inputs.PlayerActions.Movement.canceled += Movement;
        inputs.PlayerActions.Change.started += ChangePlayer;

        currentState = states[PlayerState.Idle];
    }

    private void Interaction(InputAction.CallbackContext ctx)
    {
        if (currentState == states[PlayerState.Stun]) return;
        TransitionToState(PlayerState.Interaction);
    }

    private void Movement(InputAction.CallbackContext ctx)
    {
        Vector2 movement = ctx.ReadValue<Vector2>();
        if (currentState == states[PlayerState.Stun]) return;
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
        if (currentState == states[PlayerState.Stun]) return;
        TransitionToState(PlayerState.Change);
    }
    private void OnEnable()
    {
        inputs.Enable();
    }

    private void OnDisable()
    {
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
        playerController.virtualCamera.gameObject.SetActive(true);
        PlayerController.switchedPlayer = true;
        playerController.TransitionToState(PlayerController.PlayerState.Stun);
    }

    public override void ExitState()
    {
        Debug.Log("Change state exit");
    }

    public override PlayerController.PlayerState GetNextState()
    {
        return PlayerController.PlayerState.Stun;
    }

    public override void UpdateState()
    {
        
    }
}
public class PlayerStunState : BaseState<PlayerController.PlayerState>
{
    private PlayerController playerController;

    public PlayerStunState(PlayerController.PlayerState state, PlayerController playerController) : base(state)
    {
        this.playerController = playerController;
    }

    public override void EnterState()
    {
        playerController.animator.SetBool("isWalking", false);
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
        if (!PlayerController.switchedPlayer) playerController.TransitionToState(PlayerController.PlayerState.Idle);
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
            if (!playerController.items.Contains(item) && item.isCollectable())
            {
                playerController.items.Add(item);
                InventoryManager.Inventory.AddRange(playerController.items);
                playerController.canvasManager.UpdateCanvas(InventoryManager.Inventory);
            }
        }
        else if (interactables.Length == 0)
        {
            Items item = InventoryManager.instance.GetActiveItem();
            item.Interacted();
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
        playerController.transform.position += new Vector3(movementVector.x, 0, movementVector.y) * Time.deltaTime * 1.2f;
        playerController.transform.rotation = Quaternion.Lerp(playerController.transform.rotation, Quaternion.LookRotation(new Vector3(movementVector.x, 0, movementVector.y)), Time.deltaTime * 5f);
    }
    public override PlayerController.PlayerState GetNextState()
    {
        return PlayerController.PlayerState.Idle;
    }
}
