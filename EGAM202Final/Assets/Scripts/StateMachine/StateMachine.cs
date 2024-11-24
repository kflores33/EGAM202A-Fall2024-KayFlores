using System;
using System.Collections.Generic;
using UnityEngine;

// references this video: https://youtu.be/qsIiFsddGV4?si=_ho5nntlk1SKjl-T
public abstract class StateMachine<EState> : MonoBehaviour where EState : Enum
{
    protected Dictionary<EState, BaseState<EState>> States = new Dictionary<EState, BaseState<EState>>(); // protected means only scripts of this class have access to this

    protected BaseState<EState> currentState;

    private EState queuedState;

    protected bool isTransitioningState = false;

    private void Start() 
    {
        currentState.EnterState();
    }
    private void Update() 
    {
        EState nextStateKey = currentState.GetNextState();

        if (nextStateKey.Equals(currentState.StateKey)) // use the update function of the current state if the state keys match up
        { 
            currentState.UpdateState(); 
        }
        else if (!isTransitioningState) { TransitionToState(nextStateKey); } // if the state keys do not match, go to the next state
    }

    public void TransitionToState(EState stateKey)
    {
        isTransitioningState = true;

        currentState.ExitState();
        currentState = States[stateKey];
        currentState.EnterState();

        isTransitioningState = false;
    }

    private void OnTriggerEnter(Collider other) 
    {
        currentState.OnTriggerEnter(other);
    }
    private void OnTriggerStay(Collider other) 
    { 
        currentState.OnTriggerStay(other);
    }
    private void OnTriggerExit(Collider other) 
    {
        currentState.OnTriggerExit(other);
    }
}
