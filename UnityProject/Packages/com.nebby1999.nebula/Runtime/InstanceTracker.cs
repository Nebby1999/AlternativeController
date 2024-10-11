using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Nebula
{
    /// <summary>
    /// Clase el cual se utiliza para manejar instancias de algun componente en especifico
    /// </summary>
    public static class InstanceTracker
    {
        private static class TypeData<T> where T : MonoBehaviour
        {
            public static readonly List<T> instances;

            static TypeData()
            {
                instances = new List<T>();
            }

            public static int Add(T instance)
            {
                instances.Add(instance);
                return instances.Count;
            }

            public static int Remove(T instance)
            {
                instances.Remove(instance);
                return instances.Count;
            }
        }

        /// <summary>
        /// Agrega una nueva instancia del componente de tipo <typeparamref name="T"/>
        /// </summary>
        /// <returns>El indice del nuevo componente</returns>
        public static int Add<T>(T instance) where T : MonoBehaviour
        {
            return TypeData<T>.Add(instance);
        }

        /// <summary>
        /// Remueve la <paramref name="instance"/> del tracker
        /// </summary>
        /// <returns>El indice removido</returns>
        public static int Remove<T>(T instance) where T : MonoBehaviour
        {
            return TypeData<T>.Remove(instance);
        }

        /// <summary>
        /// Consigue todas las instancias activas de tipo <typeparamref name="T"/>
        /// </summary>
        public static List<T> GetInstances<T>() where T : MonoBehaviour
        {
            return TypeData<T>.instances;
        }

        /// <summary>
        /// Existe cualquier instancia de tipo <typeparamref name="T"/>?
        /// </summary>
        public static bool Any<T>() where T : MonoBehaviour
        {
            return TypeData<T>.instances.Count > 0;
        }

        /// <summary>
        /// Retorna el primer componente de tipo <typeparamref name="T"/> que exista, o null si no existe ningun componente de tipo <typeparamref name="T"/>
        /// </summary>
        public static T FirstOrDefault<T>() where T : MonoBehaviour
        {
            if (TypeData<T>.instances.Count == 0)
            {
                return null;
            }
            return TypeData<T>.instances[0];
        }

        /// <summary>
        /// Consigue una instancia aleatoria de tipo <typeparamref name="T"/>
        /// </summary>
        /// <param name="rng">El generador de numeros random a usar, puede ser nulo.</param>
        public static T Random<T>(Xoroshiro128Plus rng = null) where T : MonoBehaviour
        {
            if (TypeData<T>.instances.Count == 0)
                return null;

            rng ??= new Xoroshiro128Plus((ulong)UnityEngine.Random.seed);
            return TypeData<T>.instances[rng.RangeInt(0, TypeData<T>.instances.Count)];
        }
    }
}