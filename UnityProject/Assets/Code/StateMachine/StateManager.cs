using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;

public abstract class StateManager<TState, TContext> : MonoBehaviour where TState : Enum where TContext : StateManager<TState, TContext>
{
    protected TContext Context { get; private set; }
    protected Dictionary<TState, State<TState, TContext>> States = new Dictionary<TState, State<TState, TContext>>();
    [SerializeField] protected State<TState, TContext> CurrentState;
    protected bool IsTransitioningState = false;
    protected abstract void InitializeStates();
    protected virtual void Awake()
    {   
        InitializeStates();
    }
    protected virtual void Start() {
        CurrentState.EnterState();
    }
    protected virtual void Update() {
        TState nextStateKey = CurrentState.GetNextState();
        if(!IsTransitioningState && nextStateKey.Equals(CurrentState.StateKey)){
            CurrentState.UpdateState();
        } else if(!IsTransitioningState) {
            TransitionToState(nextStateKey);
        }
    }
    public void TransitionToState(TState stateKey)
    {
        IsTransitioningState = true;
        CurrentState.ExitState();
        CurrentState = States[stateKey];
        CurrentState.EnterState();
        IsTransitioningState = false;
    }
    private void OnTriggerEnter(Collider other) {
        CurrentState.OnTriggerEnter(other);
    }
    private void OnTriggerStay(Collider other) {
        CurrentState.OnTriggerStay(other);
    }
    private void OnTriggerExit(Collider other) {
        CurrentState.OnTriggerExit(other);
    }
}
