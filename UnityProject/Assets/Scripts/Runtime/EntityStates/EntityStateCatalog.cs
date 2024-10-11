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
    /// <summary>
    /// Catalogo que contiene todos los estados registrados dentro del juego.
    /// 
    /// <br>Inicializado por <see cref="ACApplication.C_LoadGameContent"/></br>
    /// </summary>
    public static class EntityStateCatalog
    {
        /// <summary>
        /// Booleano que representa si el catalogo esta inicializado y esta actualmente disponible.
        /// </summary>
        public static bool initialized { get; private set; }
        private static Type[] entityStates = Array.Empty<Type>();
        private static readonly Dictionary<Type, Action<object>> instanceFieldInitializers = new Dictionary<Type, Action<object>>(); //La accion guardad en este diccionario inicializa los valores por defecto de instancia, serializado por fields marcados con Serializedfield

        /// <summary>
        /// Inicializa el <see cref="EntityStateCatalog"/>
        /// </summary>
        /// <returns>Un Coroutine el cual se puede procesar.</returns>
        internal static CoroutineTask Initialize()
        {
            return new CoroutineTask(C_InitializationCoroutine());
        }

        private static IEnumerator C_InitializationCoroutine()
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
                catch (ReflectionTypeLoadException e)
                {
                    Debug.LogWarning(e); //Añade los types que conseguimos.
                    entityStates.AddRange(e.Types);
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

        /// <summary>
        /// Inicializa un nuevo estado guardado por un <see cref="SerializableSystemType"/>
        /// </summary>
        /// <param name="serializableSystemType">El estado que vamos a inicializar.</param>
        /// <returns>El estado inicializado</returns>
        /// <exception cref="ArgumentException">Cuando el Type guardado por <paramref name="serializableSystemType"/> no es de tipo <see cref="State"/></exception>
        public static EntityState InstantiateState(SerializableSystemType serializableSystemType)
        {
            Type type = (Type)serializableSystemType;
            if (type == null || !type.IsSubclassOf(typeof(State)))
                throw new ArgumentException($"SerializableSystemType provided has a null type or does not subclass State");
            return InstantiateState(type);
        }

        /// <summary>
        /// Inicializa un nuevo estado representado por <paramref name="stateType"/>
        /// </summary>
        /// <param name="stateType">El estado que queremos inicializar</param>
        /// <returns>El estado inicializado</returns>
        public static EntityState InstantiateState(Type stateType)
        {
            if (stateType != null && stateType.IsSubclassOf(typeof(State)))
            {
                return Activator.CreateInstance(stateType) as EntityState;
            }
            Debug.LogError($"State provided is either null or does not inherit from EntityState (State: {stateType}");
            return null;
        }

        /// <summary>
        /// Inicializa los fields de instancia del estado seleccionado.
        /// </summary>
        /// <param name="entityState">El estado el cual deseamos inicializar sus fields de instancia.</param>
        public static void InitializeStateField(EntityState entityState)
        {
            if (instanceFieldInitializers.TryGetValue(entityState.GetType(), out Action<object> initializer))
            {
                initializer(entityState);
            }
        }
    }
}