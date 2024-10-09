using UnityEngine;
using System;

namespace States
{

    [Obsolete]
    public abstract class State<TState, TContext> where TState : Enum where TContext : StateManager<TState, TContext>
    {
        protected TContext Context {get; private set;}
        public TState StateKey {get; private set;}
        public State(TState key, TContext context)
        {
            StateKey = key;
            Context = context;
        }
        public abstract void EnterState();
        public abstract void ExitState();
        public abstract void UpdateState();
        public abstract TState GetNextState();
        public abstract void OnTriggerEnter(Collider other);
        public abstract void OnTriggerStay(Collider other);
        public abstract void OnTriggerExit(Collider other);
    
    }
}