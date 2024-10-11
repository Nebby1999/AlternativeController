using System;
using UnityEngine;

namespace Nebula.Serialization
{
    /// <summary>
    /// Representa una forma de serilaizar un <see cref="System.Type"/>
    /// </summary>
    [Serializable]
    public struct SerializableSystemType
    {
        [SerializeField, Tooltip("El nombre del Type que estamos serializando.")]
        private string _typeName;

        /// <summary>
        /// Esto obtiene el Type que se encuentra en este SerializableSystemType, si deseas el type actual de <see cref="SerializableSystemType"/>, ocupa "typeof(SerializableSystemType)".
        /// </summary>
        [Obsolete("This obtains the Type stored in this SerializableSystemType, if you want the actual Type of SerializableSystemType, use \"typeof(SerializableSystemType)\"")]
        public new Type GetType()
        {
            return Type.GetType(_typeName);
        }

        /// <summary>
        /// Obtiene el Type guardado en este SerializableSystemType
        /// </summary>
        public static explicit operator Type(SerializableSystemType type)
        {
    #pragma warning disable CS0618 // Type or member is obsolete
            return type.GetType();
    #pragma warning restore CS0618 // Type or member is obsolete
        }

        /// <summary>
        /// Crea un nuevo SerializableSystemType usando el <paramref name="typeName"/>
        /// </summary>
        public SerializableSystemType(string typeName) => _typeName = typeName;

        /// <summary>
        /// Crea un nuevo SerializableSystemType serializando el valor <paramref name="type"/>
        /// </summary>
        public SerializableSystemType(Type type) => _typeName = type.AssemblyQualifiedName;
    
        /// <summary>
        /// Marca que un SerializableSystemType deberia hederar de un type en especifico.
        /// </summary>
        public class RequiredBaseTypeAttribute : PropertyAttribute
        {
            /// <summary>
            /// El type que se deberia hederar
            /// </summary>
            public Type type { get; private set; }

            /// <summary>
            /// Constructor de un argumento.
            /// </summary>
            public RequiredBaseTypeAttribute(Type baseType)
            {
                type = baseType;
            }

            private RequiredBaseTypeAttribute()
            {

            }
        }
    }
}