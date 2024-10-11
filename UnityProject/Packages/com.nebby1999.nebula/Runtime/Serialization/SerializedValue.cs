using System;
using UnityEngine;
using System.Reflection;

namespace Nebula.Serialization
{
    /// <summary>
    /// Representa un valor serializado
    /// </summary>
    [Serializable]
    public struct SerializedValue : IEquatable<SerializedValue>
    {
        [Tooltip("referencia a un objeto de unity")]
        public UnityEngine.Object objectReferenceValue;

        [Tooltip("el valor serializado como un string")]
        public string stringValue;

        /// <summary>
        /// Revisa si dos Serializedvalues son iguales.
        /// </summary>
        public bool Equals(SerializedValue other)
        {
            if(string.Equals(stringValue, other.stringValue, StringComparison.OrdinalIgnoreCase))
            {
                return objectReferenceValue == other.objectReferenceValue;
            }
            return false;
        }

        /// <summary>
        /// Consigue el valor 
        /// </summary>
        /// <param name="fieldInfo"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public object GetValue(FieldInfo fieldInfo)
        {
            Type fieldType = fieldInfo.FieldType;
            if(typeof(UnityEngine.Object).IsAssignableFrom(fieldType)) 
            {
                if(objectReferenceValue != null)
                {
                    Type type = objectReferenceValue.GetType();
                    if (!fieldType.IsAssignableFrom(type))
                    {
                        if (type == typeof(UnityEngine.Object))
                        {
                            return null;
                        }
                        throw new Exception($"Value \"{objectReferenceValue}\" of type \"{type}\" is not suitable for field \"{fieldInfo.DeclaringType.Name}.{fieldInfo.Name}\"");
                    }
                }
                return objectReferenceValue;
            }

            if(stringValue != null)
            {
                if(StringSerializer.CanSerializeType(fieldType))
                {
                    try
                    {
                        return StringSerializer.Deserialize(fieldType, stringValue);
                    }
                    catch(Exception e)
                    {
                        Debug.LogWarning($"Could not deserialize field \"{fieldInfo.DeclaringType}.{fieldInfo.Name}\": {e}");
                    }
                }
            }

            if (fieldType.IsValueType)
                return Activator.CreateInstance(fieldType);

            return null;
        }

        public void SetValue(FieldInfo fieldInfo, object newValue)
        {
            try
            {
                stringValue = null;
                objectReferenceValue = null;
                Type fieldType = fieldInfo.FieldType;
                if(typeof(UnityEngine.Object).IsAssignableFrom(fieldType))
                {
                    objectReferenceValue = (UnityEngine.Object)newValue;
                    return;
                }
                if(StringSerializer.CanSerializeType(fieldType))
                {
                    stringValue = StringSerializer.Serialize(fieldType, newValue);
                    return;
                }
                throw new Exception($"Unrecognized type {fieldType.FullName}");
            }
            catch(Exception e)
            {
                Debug.LogError($"Could not serialize field \"{fieldInfo.DeclaringType.FullName}.{fieldInfo.Name}\": {e}");
            }
        }

        public void PurgeUnityNull()
        {
            if (objectReferenceValue != null && !objectReferenceValue)
            {
                objectReferenceValue = null;
            }
        }

        public static bool CanSerializeField(FieldInfo fieldInfo)
        {
            Type fieldType = fieldInfo.FieldType;
            if (!typeof(UnityEngine.Object).IsAssignableFrom(fieldType) && !StringSerializer.CanSerializeType(fieldType))
                return false;

            if(fieldInfo.IsStatic && fieldInfo.IsPublic)
            {
                return true;
            }

            bool serializeFieldAttribute = fieldInfo.GetCustomAttribute<SerializeField>() != null;
            bool nonSerializedAttribute = fieldInfo.GetCustomAttribute<NonSerializedAttribute>() != null;

            return serializeFieldAttribute && !nonSerializedAttribute;
        }
    }
}