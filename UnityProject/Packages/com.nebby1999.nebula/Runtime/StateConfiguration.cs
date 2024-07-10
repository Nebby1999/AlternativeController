using EntityStates;
using Nebula.Serialization;
using System.Collections.Generic;
using System.Reflection;
using System;
using UnityEngine;

namespace Nebula
{
    public abstract class StateConfiguration : NebulaScriptableObject
    {
        [SerializableSystemType.RequiredBaseType(typeof(State))]
        public SerializableSystemType stateTypeToConfig;
        public SerializedFieldCollection fieldCollection;

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

#if UNITY_EDITOR
        [ContextMenu("Editor PlayMode-Test serialized values")]
        public virtual void RuntimeTestSerializedValues()
        {
            if (!Application.isPlaying)
                return;

            Type targetType = (Type)stateTypeToConfig;
            if (targetType == null)
                return;

            State state = InstantiateState();
            foreach (FieldInfo field in state.GetType().GetFields())
            {
                Debug.Log($"{field.Name} ({field.FieldType.Name}): {field.GetValue(state)}");
            }
        }

        public abstract State InstantiateState();
#endif

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

            List<(FieldInfo, object)> fieldValuePair = new List<(FieldInfo, object)>();
            foreach (SerializedField serializedField in fieldCollection.serializedFields)
            {
                try
                {
                    FieldInfo field = _targetType.GetField(serializedField.fieldName);
                    if (field != null && ShouldSerializeField(field))
                    {
                        fieldValuePair.Add((field, serializedField.serializedValue.GetValue(field)));
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }
            }

            if (fieldValuePair.Count == 0)
                return null;

            return InitializeObject;
            bool ShouldSerializeField(FieldInfo field)
            {
                var hasSerializeField = field.GetCustomAttribute<SerializeField>() != null;
                var hasHideInInspector = field.GetCustomAttribute<NonSerializedAttribute>() != null;

                return hasSerializeField && !hasHideInInspector;
            }

            void InitializeObject(object obj)
            {
                foreach (var pair in fieldValuePair)
                {
                    pair.Item1.SetValue(obj, pair.Item2);
                }
            }
        }

        protected override void Awake()
        {
            base.Awake();
            fieldCollection.PurgeUnityPseudoNull();
        }
    }
}