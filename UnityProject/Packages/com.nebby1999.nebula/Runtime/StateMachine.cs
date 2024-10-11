using EntityStates;
using Nebula.Serialization;
using System;
using UnityEngine;

namespace Nebula
{
    /// <summary>
    /// Una <see cref="StateMachine"/> es un componente que te permite correr clases que heredan de <see cref="State"/>, estas clases te permiten tener multiples maquinas controlando distintas partes de un GameObject
    /// <para></para>
    /// Esta clase se debe heredar para utilizar correctamente
    /// </summary>
    public abstract class StateMachine : MonoBehaviour
    {
        /// <summary>
        /// El nombre de esta maquina de estado
        /// </summary>
        public string stateMachineName;

        /// <summary>
        /// Cuando la maquina de estado comienza, este estado es su estado inicial
        /// </summary>
        public abstract SerializableSystemType initialStateType { get; }

        /// <summary>
        /// Cuando <see cref="SetNextStateToMain"/> es llamado, este es el nuevo estado de la maquina.
        /// </summary>
        public abstract SerializableSystemType mainStateType { get; }

        /// <summary>
        /// El nuevo estado que deberiamos cambiar
        /// </summary>
        public State newState { get; private set; }

        /// <summary>
        /// El estado actual que esta corriendo
        /// </summary>
        public State currentState
        {
            get => _currentState;
            private set
            {
                if (value == null)
                    return;

                _currentState?.ModifyNextState(value);
                _currentState?.OnExit();
                _currentState = value;
                _currentState.OnEnter();
            }
        }
        private State _currentState;

        /// <summary>
        /// Un numero identificador unico dentro del GameObject para esta maquina de estado
        /// </summary>
        public int stateMachineID { get; private set; }

        /// <summary>
        /// Crea un nuevo <see cref="State"/> del <paramref name="stateType"/> especifico
        /// </summary>
        /// <param name="stateType">El estado que se va a crear</param>
        /// <returns>La instancia del estado</returns>
        protected abstract State InitializeState(Type stateType);

        /// <summary>
        /// Asigna el valor de <see cref="stateMachineID"/> y coloca <see cref="currentState"/> en <see cref="Uninitialized"/>
        /// </summary>
        protected virtual void Awake()
        {
            stateMachineID = stateMachineName.GetHashCode();
            currentState = new Uninitialized();
            currentState.outer = this;
        }

        /// <summary>
        /// Asigna el valor de <see cref="currentState"/> a <see cref="initialStateType"/> SI <see cref="currentState"/> es de valor <see cref="Uninitialized"/>
        /// </summary>
        protected virtual void Start()
        {
            var initState = (Type)initialStateType;
            if(currentState is Uninitialized && initState != null)
            {
                SetState(InitializeState(initState));
            }
        }

        /// <summary>
        /// Asigna un nuevo estado a esta maquina de estado
        /// </summary>
        /// <param name="newState">El nuevo estado para esta maquina</param>
        /// <exception cref="NullReferenceException">Cuando <paramref name="newState"/> es null</exception>
        protected virtual void SetState(State newState)
        {
            if (newState == null)
                throw new NullReferenceException("newState is null");

            newState.outer = this;
            this.newState = null;
            currentState = newState;
        }

        /// <summary>
        /// Asigna un nuevo estado a ejectuar en el siguiente frame
        /// </summary>
        /// <param name="newNextState">El nuevo estado a usar a partir del siguiente frame</param>
        /// <exception cref="NullReferenceException">Cuando <paramref name="newNextState"/> es null</exception>
        public virtual void SetNextState(State newNextState)
        {
            if (newNextState == null)
                throw new NullReferenceException("newNextState is null");

            newNextState.outer = this;
            newState = newNextState;
        }

        /// <summary>
        /// Coloca el estado de esta maquina a <see cref="mainStateType"/>
        /// </summary>
        public virtual void SetNextStateToMain()
        {
            SetNextState(InitializeState((Type)mainStateType));
        }

        /// <summary>
        /// Corre el metodo <see cref="State.Update"/> de <see cref="currentState"/>
        /// </summary>
        protected virtual void Update()
        {
            currentState?.Update();
        }

        /// <summary>
        /// Corre el metodo <see cref="State.FixedUpdate"/> de <see cref="currentState"/>, si <see cref="newState"/> tiene un valor, se cambia el estado actual de la maquina.
        /// </summary>
        protected virtual void FixedUpdate()
        {
            currentState?.FixedUpdate();

            if (newState != null)
                SetState(newState);
        }

        /// <summary>
        /// Corre el metodo <see cref="State.OnExit"/> de <see cref="currentState"/>
        /// </summary>
        protected virtual void OnDestroy()
        {
            currentState?.OnExit();
        }

        /// <summary>
        /// Encuentra una maquina de estado dentro de <paramref name="obj"/> del nombre <paramref name="name"/>
        /// </summary>
        /// <typeparam name="TSM">La sub-clase de maquina a retornar</typeparam>
        /// <param name="obj">El GameObject el cual vamos a buscar la maquina</param>
        /// <param name="name">El nombre de la maquina</param>
        /// <returns>La maquina de estado, o null si no se encuentra</returns>
        public static TSM FindStateMachineByName<TSM>(GameObject obj, string name) where TSM : StateMachine
        {
            int hashCode = name.GetHashCode();
            return FindStateMachineByHashCode<TSM>(obj, hashCode);
        }

        /// <summary>
        /// Encuentra una maquina de estado dentro de <paramref name="obj"/> cuyo <see cref="stateMachineID"/> es igual a <paramref name="hashCode"/>
        /// </summary>
        /// <typeparam name="TSM">La sub-clase de maquina a retornar</typeparam>
        /// <param name="obj">El GameObject el cual vamos a buscar la maquina</param>
        /// <param name="hashCode">El identificador unico de la maquina</param>
        /// <returns>La maquina de estado, o null si no se encuentra</returns>
        public static TSM FindStateMachineByHashCode<TSM>(GameObject obj, int hashCode) where TSM : StateMachine
        {
            var stateMachines = obj.GetComponents<TSM>();
            foreach (var stateMachine in stateMachines)
            {
                if (stateMachine.stateMachineID == hashCode)
                    return stateMachine;
            }
            return null;
        }
    }
}