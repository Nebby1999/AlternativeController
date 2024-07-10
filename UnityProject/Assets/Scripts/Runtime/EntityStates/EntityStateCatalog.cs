using EntityStates;
using Nebula;
using Nebula.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace AC
{
    public static class EntityStateCatalog
    {
        public static bool initialized { get; private set; }
        private static Type[] entityStates = Array.Empty<Type>();
        private static readonly Dictionary<Type, Action<object>> instanceFieldInitializers = new Dictionary<Type, Action<object>>();

        internal static IEnumerator Initialize()
        {
            var obj = Addressables.LoadAssetsAsync<EntityStateConfiguration>("EntityStateConfigurations", _ => { });
            while (!obj.IsDone)
                yield return null;

            entityStates = LoadEntityStates();

            foreach (EntityStateConfiguration config in obj.Result)
            {
                ApplyStateConfig(config);
            }
        }

        private static Type[] LoadEntityStates()
        {
            List<Type> entityStates = new List<Type>();
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                try
                {
                    var typeStates = assembly.GetTypesSafe()
                        .Where(t => t.IsSubclassOf(typeof(EntityStates.State)));

                    entityStates.AddRange(typeStates);
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }
            }
            return entityStates.ToArray();
        }

        private static void ApplyStateConfig(EntityStateConfiguration config)
        {
            Type targetType = (Type)config.stateTypeToConfig;
            if (targetType == null)
                return;

            if (!entityStates.Contains(targetType))
                return;

            config.ApplyStaticConfiguration();
            Action<object> instanceInitializer = config.CreateInstanceInitializer();
            if (instanceInitializer == null)
            {
                instanceFieldInitializers.Remove(targetType);
            }
            else
            {
                instanceFieldInitializers[targetType] = instanceInitializer;
            }
        }

        public static EntityState InstantiateState(SerializableSystemType serializableSystemType)
        {
            Type type = (Type)serializableSystemType;
            if (type == null || !type.IsSubclassOf(typeof(State)))
                throw new ArgumentException($"SerializableSystemType provided has a null type or does not subclass State");
            return InstantiateState(type);
        }

        public static EntityState InstantiateState(Type stateType)
        {
            if (stateType != null && stateType.IsSubclassOf(typeof(State)))
            {
                return Activator.CreateInstance(stateType) as EntityState;
            }
            Debug.LogError($"State provided is either null or does not inherit from EntityState (State: {stateType}");
            return null;
        }

        public static void InitializeStateField(EntityState entityState)
        {
            if (instanceFieldInitializers.TryGetValue(entityState.GetType(), out Action<object> initializer))
            {
                initializer(entityState);
            }
        }
    }
}