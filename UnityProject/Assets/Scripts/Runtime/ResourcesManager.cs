using Nebula;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AC
{

    /// <summary>
    /// Representa un contenedor generico de Recursos.
    /// </summary>
    public class ResourcesManager : MonoBehaviour
    {
        /// <summary>
        /// Los tipos de recursos guardados en este Manager
        /// </summary>
        public HashSet<ResourceIndex> resourceTypesStored = new HashSet<ResourceIndex>();

        /// <summary>
        /// La cantidad total de recursos guardados.
        /// </summary>
        public float totalResourcesCont
        {
            get
            {
                float total = 0;
                foreach(var f in _resources)
                {
                    total += f;
                }
                return total;
            }
        }
        private float[] _resources;

        /// <summary>
        /// Evento alzado cuando los recursos se agotan.
        /// </summary>
        public event Action onEmpty;

        private void Awake()
        {
            _resources = new float[ResourceCatalog.resourceCount];
        }

        /// <summary>
        /// Obtiene la cantidad de recursos de tipo <paramref name="resourceDef"/> guardados.
        /// </summary>
        /// <param name="resourceDef">El tipo de recurso</param>
        /// <returns>La cantidad del recurso</returns>
        public float GetResourceCount(ResourceDef resourceDef) => GetResourceCount(resourceDef ? resourceDef.resourceIndex : ResourceIndex.None);

        /// <summary>
        /// Obtiene la cantidad de recursos de tipo <paramref name="index"/> guardados.
        /// </summary>
        /// <param name="index">El tipo de recurso</param>
        /// <returns>La cantidad del recurso</returns>
        public float GetResourceCount(ResourceIndex index) => ArrayUtils.GetSafe(ref _resources, (int)index);


        /// <summary>
        /// Carga <paramref name="amount"/> cantidad de recursos de tipo <paramref name="resourceDef"/>.
        /// </summary>
        /// <param name="resourceDef">El tipo de recurso</param>
        /// <param name="amount">La cantidad de recurso a Cargar</param>
        /// <returns>Verdadero si el proceso de carga funciono, si no, retorna falso</returns>
        public bool LoadResource(ResourceDef resourceDef, float amount) => LoadResource(resourceDef ? resourceDef.resourceIndex : ResourceIndex.None, amount);

        /// <summary>
        /// Carga <paramref name="amount"/> cantidad de recursos de tipo <paramref name="resourceIndex"/>.
        /// </summary>
        /// <param name="resourceIndex">El tipo de recurso</param>
        /// <param name="amount">La cantidad de recurso a Cargar</param>
        /// <returns>Verdadero si el proceso de carga funciono, si no, retorna falso</returns>
        public bool LoadResource(ResourceIndex resourceIndex, float amount)
        {
            if (resourceIndex == ResourceIndex.None)
                return false;

            int index = (int)resourceIndex;
            _resources[index] += amount;

            if(!resourceTypesStored.Contains(resourceIndex) && _resources[index] > 0)
                resourceTypesStored.Add(resourceIndex);

            return true;
        }

        /// <summary>
        /// Descarga <paramref name="amount"/> cantidad de recursos de tipo <paramref name="resourceDef"/>.
        /// </summary>
        /// <param name="resourceDef">El tipo de recurso</param>
        /// <param name="amount">La cantidad de recurso a Descargar</param>
        /// <returns>Verdadero si el proceso de descarga funciono, si no, retorna falso</returns>
        public bool UnloadResource(ResourceDef resourceDef, float amount) => UnloadResource(resourceDef ? resourceDef.resourceIndex : ResourceIndex.None, amount);

        /// <summary>
        /// Descarga <paramref name="amount"/> cantidad de recursos de tipo <paramref name="resourceIndex"/>.
        /// </summary>
        /// <param name="resourceIndex">El tipo de recurso</param>
        /// <param name="amount">La cantidad de recurso a Descargar</param>
        /// <returns>Verdadero si el proceso de descarga funciono, si no, retorna falso</returns>
        public bool UnloadResource(ResourceIndex resourceIndex, float amount)
        {
            if (resourceIndex == ResourceIndex.None)
                return false;

            int index = (int)resourceIndex;
            _resources[index] -= amount;

            if (_resources[index] < 0) 
                _resources[index] = 0;

            if (resourceTypesStored.Contains(resourceIndex) && _resources[index] == 0)
                resourceTypesStored.Remove(resourceIndex);

            return true;
        }
    }
}