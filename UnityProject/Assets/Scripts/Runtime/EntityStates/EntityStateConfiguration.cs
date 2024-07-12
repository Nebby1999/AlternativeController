using EntityStates;
using Nebula;
using Nebula.Serialization;
using System;
using UnityEngine;

namespace AC
{
    [CreateAssetMenu(fileName = "New EntityStateConfiguration", menuName = "AC/EntityStateConfiguration")]
    public class EntityStateConfiguration : StateConfiguration
    {
        public override SerializableSystemType stateTypeToConfig => stateTypeToConfig;
        [SerializeField] private SerializableSystemType _stateTypeToConfig;
        public override State InstantiateState()
        {
            return EntityStateCatalog.InstantiateState((Type)stateTypeToConfig);
        }
    }
}