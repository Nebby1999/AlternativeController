using EntityStates;
using Nebula.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AC
{
    /// <summary>
    /// See <see cref="Nebula.StateMachine"/>
    /// </summary>
    public class EntityStateMachine : Nebula.StateMachine
    {
        public CommonComponentLocator componentLocator { get; private set; }
        public override SerializableSystemType initialStateType => _initialStateType;
        [SerializeField, SerializableSystemType.RequiredBaseType(typeof(EntityState)), Tooltip("When the object is first instantiated, this is it's initial state.")] private SerializableSystemType _initialStateType;

        public override SerializableSystemType mainStateType => _mainStateType;
        [SerializeField, SerializableSystemType.RequiredBaseType(typeof(EntityState)), Tooltip("Whenever a state uses SetNextStateToMain, the machine's state is set to this state.")] private SerializableSystemType _mainStateType;

        protected override void Awake()
        {
            base.Awake();
            componentLocator = new CommonComponentLocator(gameObject);
        }
        protected override State InitializeState(Type stateType)
        {
            return EntityStateCatalog.InstantiateState(stateType);
        }

        public readonly struct CommonComponentLocator
        {
            public readonly CharacterBody characterBody;
            public readonly HealthComponent healthComponent;
            public readonly InputBank inputBank;
            public readonly Rigidbody2DCharacterController rigidbody2DCharacterController;
            public CommonComponentLocator(GameObject gameObject)
            {
                characterBody = gameObject.GetComponent<CharacterBody>();
                healthComponent = gameObject.GetComponent<HealthComponent>();
                inputBank = gameObject.GetComponent<InputBank>();
                rigidbody2DCharacterController = gameObject.GetComponent<Rigidbody2DCharacterController>();
            }
        }
    }
}
