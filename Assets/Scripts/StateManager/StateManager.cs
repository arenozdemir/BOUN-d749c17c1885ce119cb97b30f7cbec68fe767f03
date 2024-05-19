using System.Collections.Generic;
using UnityEngine;

public abstract class StateManager<EState> : MonoBehaviour where EState : System.Enum
{
    protected Dictionary<EState, BaseState<EState>> states = new Dictionary<EState, BaseState<EState>>();
    protected BaseState<EState> currentState;

    protected bool isTransationingState = false;
    protected Inputs inputs;
    private void Awake()
    {
        inputs = new Inputs();
    }
    void Start()
    {
        currentState.EnterState();
    }

    void Update()
    {
        currentState.movementVector = inputs.PlayerActions.Movement.ReadValue<Vector2>().normalized;
        EState nextStateKey = currentState.GetNextState();
        if (!isTransationingState && !nextStateKey.Equals(currentState.StateKey))
        {
            currentState.UpdateState();
        }
        else if (!isTransationingState)
        {
            TransitionToState(nextStateKey);
        }
    }
    
    public void TransitionToState(EState stateKey)
    {
        isTransationingState = true;
        currentState.ExitState();
        currentState = states[stateKey];
        currentState.EnterState();
        isTransationingState = false;
    }
}
