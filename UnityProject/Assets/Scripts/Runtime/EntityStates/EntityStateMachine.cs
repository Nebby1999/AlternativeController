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
            public CommonComponentLocator(GameObject gameObject)
            {

            }
        }
    }
}
