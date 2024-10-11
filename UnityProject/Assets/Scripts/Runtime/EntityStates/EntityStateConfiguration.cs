using EntityStates;
using Nebula;
using Nebula.Serialization;
using System;
using UnityEngine;

namespace AC
{
    /// <summary>
    /// <inheritdoc cref="StateConfiguration"/>
    /// </summary>
    [CreateAssetMenu(fileName = "New EntityStateConfiguration", menuName = "AC/EntityStateConfiguration")]
    public class EntityStateConfiguration : StateConfiguration
    {
        /// <summary>
        /// El <see cref="EntityState"/> que este configurador esta configurando
        /// </summary>
        public override SerializableSystemType stateTypeToConfig => _stateTypeToConfig;
        [SerializableSystemType.RequiredBaseType(typeof(EntityState))]
        [Tooltip("El estado que este configurador esta configurando.")]
        [SerializeField] private SerializableSystemType _stateTypeToConfig;

        /// <summary>
        /// Metodo para inicializar el estado referenciado por este <see cref="EntityStateConfiguration"/>
        /// </summary>
        /// <returns>El Estado</returns>
        public override State InstantiateState()
        {
            return EntityStateCatalog.InstantiateState((Type)stateTypeToConfig);
        }
    }
}