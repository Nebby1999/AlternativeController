using EntityStates;
using Nebula.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

namespace AC
{
    /// <summary>
    /// <inheritdoc cref="Nebula.StateMachine"/>
    /// </summary>
    public class EntityStateMachine : Nebula.StateMachine
    {
        /// <summary>
        /// Propiedad utilizada para conseguir componentes de acceso comun entre varios estados.
        /// </summary>
        public CommonComponentLocator componentLocator { get; private set; }

        /// <summary>
        /// El estado inicial que esta maquina deberia usar, este estado es usado cuando el componente es activado.
        /// </summary>
        public override SerializableSystemType initialStateType => _initialStateType;
        [SerializeField, SerializableSystemType.RequiredBaseType(typeof(EntityState))]
        [Tooltip("Cuando la maquina es creada, este es su estado inicial.")]
        private SerializableSystemType _initialStateType;

        /// <summary>
        /// El estado principal que esta maquina deberia usar, este estado es usado cuando se llama <see cref="Nebula.StateMachine.SetNextStateToMain"/>
        /// </summary>
        public override SerializableSystemType mainStateType => _mainStateType;
        [SerializeField, SerializableSystemType.RequiredBaseType(typeof(EntityState))]
        [Tooltip("Cuando se llama a \"SetNextStateToMain\", este estado sera usado.")] 
        private SerializableSystemType _mainStateType;

        protected override void Awake()
        {
            base.Awake();
            componentLocator = new CommonComponentLocator(gameObject);
        }

        protected override State InitializeState(Type stateType)
        {
            return EntityStateCatalog.InstantiateState(stateType);
        }

        /// <summary>
        /// Revisa si el estado actual puede ser interrumpido por el valor de <paramref name="priority"/>
        /// </summary>
        /// <param name="priority">La prioridad que esta intentando interrumpir el estado</param>
        /// <returns>Verdadero si el estado actual puede ser interrumpido</returns>
        public bool CanInterruptState(InterruptPriority priority)
        {
            EntityState state = (EntityState)(newState ?? currentState);
            return state.GetMinimumInterruptPriority() <= priority;
        }

        /// <summary>
        /// Estructura el cual contiene componentes en comun de un objeto.
        /// </summary>
        public readonly struct CommonComponentLocator
        {
            /// <summary>
            /// El <see cref="CharacterBody"/> de la maquina de estado.
            /// </summary>
            public readonly CharacterBody characterBody;

            /// <summary>
            /// El <see cref="HealthComponent"/> de la maquina de estado.
            /// </summary>
            public readonly HealthComponent healthComponent;

            /// <summary>
            /// El <see cref="InputBank"/> de la maquina de estado.
            /// </summary>
            public readonly InputBank inputBank;

            /// <summary>
            /// El <see cref="Rigidbody2DCharacterController"/> de la maquina de estado.
            /// </summary>
            public readonly Rigidbody2DCharacterController rigidbody2DCharacterController;

            /// <summary>
            /// El <see cref="SkillManager"/> de la maquina de estado.
            /// </summary>
            public readonly SkillManager skillManager;

            /// <summary>
            /// Constructor para un <see cref="CommonComponentLocator"/>
            /// </summary>
            /// <param name="gameObject">El GameObject el cual vamos a localizar sus componentes.</param>
            public CommonComponentLocator(GameObject gameObject)
            {
                characterBody = gameObject.GetComponent<CharacterBody>();
                healthComponent = gameObject.GetComponent<HealthComponent>();
                inputBank = gameObject.GetComponent<InputBank>();
                rigidbody2DCharacterController = gameObject.GetComponent<Rigidbody2DCharacterController>();
                skillManager = gameObject.GetComponent<SkillManager>();
            }
        }
    }
}
