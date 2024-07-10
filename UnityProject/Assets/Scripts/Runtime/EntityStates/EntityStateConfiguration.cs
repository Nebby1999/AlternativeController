using EntityStates;
using Nebula;
using System;
using UnityEngine;

namespace AC
{
    [CreateAssetMenu(fileName = "New EntityStateConfiguration", menuName = "AC/EntityStateConfiguration")]
    public class EntityStateConfiguration : StateConfiguration
    {
        public override State InstantiateState()
        {
            return EntityStateCatalog.InstantiateState((Type)stateTypeToConfig);
        }
    }
}