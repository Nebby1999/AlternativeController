using System;
using UnityEngine;

namespace Nebula.Serialization
{
    /// <summary>
    /// Representa una coleccion de <see cref="SerializedField"/>
    /// </summary>
    [Serializable]
    public struct SerializedFieldCollection
    {
        [Tooltip("Los Serializedfield siendo serializados.")]
        public SerializedField[] serializedFields;

        /// <summary>
        /// Consigue o crea un <see cref="SerializedField"/> de nombre <paramref name="fieldName"/>
        /// </summary>
        public ref SerializedField GetOrCreateField(string fieldName)
        {
            if(serializedFields == null)
            {
                serializedFields = Array.Empty<SerializedField>();
            }

            for(int i = 0; i < serializedFields.Length; i++)
            {
                ref SerializedField field = ref serializedFields[i];
                if(field.fieldName.Equals(fieldName, StringComparison.Ordinal))
                {
                    return ref field;
                }
            }

            SerializedField newField = default;
            newField.fieldName = fieldName;
            ArrayUtils.Append(ref serializedFields, newField);
            ref SerializedField reference = ref serializedFields[serializedFields.Length - 1];
            return ref reference;
        }

        /// <summary>
        /// Purga cualquier valor de un <see cref="SerializedField"/> cuyo <see cref="SerializedValue.objectReferenceValue"/> sea un null de c++, pero no un null de C#
        /// </summary>
        public void PurgeUnityPseudoNull()
        {
            if (serializedFields == null)
                serializedFields = Array.Empty<SerializedField>();

            for (int i = 0; i < serializedFields.Length; i++)
            {
                serializedFields[i].serializedValue.PurgeUnityNull();
            }
        }
    }
}