using System;
using UnityEngine;

namespace Nebula.Serialization
{
    /// <summary>
    /// Representa un field que esta serializado.
    /// </summary>
    [Serializable]
    public struct SerializedField
    {
        [Tooltip("El nombre del field serializado")]
        public string fieldName;
        [Tooltip("El valor serializado")]
        public SerializedValue serializedValue;
    }
}