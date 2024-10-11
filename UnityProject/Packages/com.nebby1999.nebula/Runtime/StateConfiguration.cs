using EntityStates;
using Nebula.Serialization;
using System.Collections.Generic;
using System.Reflection;
using System;
using UnityEngine;

namespace Nebula
{
    /// <summary>
    /// Un <see cref="StateConfiguration"/> es un ScriptableObject el cual se utiliza para serializar los valores dentro de una clase <see cref="State"/>. Valores publicos y estacicos serian serializados, mientras que valores publicos de instancia con el attributo <see cref="SerializeField"/> tambien seran serializados.
    /// </summary>
    public abstract class StateConfiguration : NebulaScriptableObject
    {
        /// <summary>
        /// El <see cref="Type"/> de tipo <see cref="State"/> a Configurar
        /// </summary>
        public abstract SerializableSystemType stateTypeToConfig { get; }

        /// <summary>
        /// La coleccion de fields del estado
        /// </summary>
        public SerializedFieldCollection fieldCollection;

        /// <summary>
        /// Asigna el nombre de este objeto al nombre completo del estado que se esta configurando
        /// </summary>
        [ContextMenu("Set name to targetType name")]
        public virtual void SetNameToTargetTypeName()
        {
            Type _targetType = (Type)stateTypeToConfig;
            if (_targetType == null)
                return;

            cachedName = _targetType.FullName;
#if UNITY_EDITOR
            UnityEditor.AssetDatabase.RenameAsset(UnityEditor.AssetDatabase.GetAssetPath(this), _targetType.FullName);
#endif
        }

        /// <summary>
        /// Crea el estado guardado en <see cref="stateTypeToConfig"/>
        /// </summary>
        /// <returns>El estado</returns>
        public abstract State InstantiateState();

        /// <summary>
        /// Aplica los valores estaticos guardados en esta configuracion a <see cref="stateTypeToConfig"/>
        /// </summary>
        public virtual void ApplyStaticConfiguration()
        {
            if (!Application.isPlaying)
                return;

            Type _targetType = (Type)stateTypeToConfig;
            if (_targetType == null)
            {
                Debug.LogError($"{this} has an invalid TargetType set! (targetType.TypeName value: {_targetType})");
                return;
            }

            foreach (SerializedField field in fieldCollection.serializedFields)
            {
                try
                {
                    FieldInfo fieldInfo = _targetType.GetField(field.fieldName, BindingFlags.Public | BindingFlags.Static);
                    if (fieldInfo == null)
                    {
                        continue;
                    }

                    var serializedValueForfield = field.serializedValue.GetValue(fieldInfo);
                    if (serializedValueForfield != null)
                    {
                        fieldInfo.SetValue(fieldInfo, serializedValueForfield);
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }
            }
        }

        /// <summary>
        /// Crea un metodo el cual asigna valores a los fields de instancia de <see cref="stateTypeToConfig"/>
        /// </summary>
        /// <returns>Una accion el cual asigna los valores de instancia, el objeto a darle a la accion deberia ser una instancia de <see cref="stateTypeToConfig"/></returns>
        public virtual Action<object> CreateInstanceInitializer()
        {
            if (!Application.isPlaying)
                return null;

            Type _targetType = (Type)stateTypeToConfig;
            if (_targetType == null)
            {
                Debug.LogError($"{this} has an invalid TargetType set! (targetType.TypeName value: {_targetType})");
                return null;
            }

            SerializedField[] serializedFields = fieldCollection.serializedFields;
            List<(FieldInfo, object)> fieldValuePair = new List<(FieldInfo, object)>();
            for(int i = 0; i < serializedFields.Length; i++)
            {
                ref SerializedField reference = ref serializedFields[i];
                try
                {
                    FieldInfo field = _targetType.GetField(reference.fieldName, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
                    if(!(field == null) && FieldPassesFilter(field))
                    {
                        fieldValuePair.Add((field, reference.serializedValue.GetValue(field)));
                    }
                }
                catch(Exception msg)
                {
                    Debug.LogError(msg);
                }
            }

            if (fieldValuePair.Count == 0)
                return null;

            return InitializeObject;
            bool FieldPassesFilter(FieldInfo field)
            {
                return field.GetCustomAttribute<SerializeField>() != null;
            }

            void InitializeObject(object obj)
            {
                foreach (var pair in fieldValuePair)
                {
                    pair.Item1.SetValue(obj, pair.Item2);
                }
            }
        }

        /// <summary>
        /// Purga fields dento de <see cref="fieldCollection"/> que son "PseudoNull", osease, su valor en C# no es null pero su valor en C++ si lo es.
        /// </summary>
        protected override void Awake()
        {
            base.Awake();
            fieldCollection.PurgeUnityPseudoNull();
        }
    }
}