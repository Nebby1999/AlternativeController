using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace AC
{
    /// <summary>
    /// Catalogo que contiene y maneja todos los recursos del juego.
    /// 
    /// <br>Inicializado por <see cref="ACApplication.C_LoadGameContent"/></br>
    /// </summary>
    public static class ResourceCatalog
    {
        /// <summary>
        /// Retorna la cantidad total de <see cref="ResourceDef"/>s en el catalogo.
        /// </summary>
        public static int resourceCount => _resourceDefs.Length;
        public static ReadOnlyCollection<ResourceDef> resourceDefs;
        private static ResourceDef[] _resourceDefs = Array.Empty<ResourceDef>();
        //Un diccionario que se usa para conseguir un ResourceIndex a partir del nombre del recurso.
        private static Dictionary<string, ResourceIndex> _resourceNameToIndex = new Dictionary<string, ResourceIndex>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Evento el cual uno se puede subscribir para correr codigo cuando el Catalog completa su inicializacion.
        /// </summary>
        public static event Action onAvailable
        {
            add
            {
                if (initialized)
                {
                    value();
                    return;
                }
                _onAvailable += value;
            }
            remove
            {
                _onAvailable -= value;
            }
        }
        private static event Action _onAvailable;

        /// <summary>
        /// Booleano que representa si el catalogo esta inicializado y esta actualmente disponible.
        /// </summary>
        public static bool initialized { get; private set; }

        /// <summary>
        /// Retorna el <see cref="ResourceDef"/> asociado con el indice <paramref name="index"/>. Un <see cref="ResourceIndex"/> se puede conseguir usando <see cref="FindResource(string)"/>
        /// </summary>
        /// <param name="index">El indice a usar</param>
        /// <returns>El ResourceDef asociado al indice, si <paramref name="index"/> es <see cref="ResourceIndex.None"/>, esta fuera del rango aceptable, o el catalogo no esta inicializado, retorna null.</returns>
        public static ResourceDef GetResourceDef(ResourceIndex index)
        {
            if (!initialized)
                return null;

            int indexAsInt = (int)index;
            if (index == ResourceIndex.None || indexAsInt > resourceCount)
                return null;

            return _resourceDefs[indexAsInt];
        }

        /// <summary>
        /// Encuentra el indice de un recurso en especifico, el <see cref="ResourceDef"/> del recurso se puede conseguir usando <see cref="GetResourceDef(ResourceIndex)"/>
        /// </summary>
        /// <param name="resourceDefName">El nombre del recurso</param>
        /// <returns>Un indice valido, o <see cref="ResourceIndex.None"/> si no se encontro un recurso con el nombre especificado, o si el catalogo no esta inicializado.</returns>
        public static ResourceIndex FindResource(string resourceDefName)
        {
            if (!initialized)
                return ResourceIndex.None;

            return _resourceNameToIndex.TryGetValue(resourceDefName, out var index) ? index : ResourceIndex.None;
        }

        /// <summary>
        /// Inicializa el <see cref="ResourceCatalog"/>
        /// </summary>
        /// <returns>Un Coroutine el cual se puede procesar.</returns>
        internal static CoroutineTask Initialize()
        {
            if (initialized)
                return null;

            initialized = true;
            return new CoroutineTask(C_InitializationCoroutine());
        }

        private static IEnumerator C_InitializationCoroutine()
        {
            int invalidCount = 0;

            var task = Addressables.LoadAssetsAsync<ResourceDef>("ResourceDefs", NameCheck);

            while (!task.IsDone)
                yield return null;

            ResourceDef[] resources = task.Result.ToArray();
            _resourceDefs = new ResourceDef[resources.Length];

            for (int i = 0; i < resources.Length; i++)
            {
                var resourceDef = resources[i];

                if (string.IsNullOrEmpty(resourceDef.cachedName))
                {
                    Debug.LogWarning("Invalid resourceDef name, using generic name.", resourceDef);
                    resourceDef.cachedName = "RESOURCEDEF_" + invalidCount;
                    invalidCount++;
                }

                ResourceIndex newIndex = (ResourceIndex)i;
                resourceDef.resourceIndex = newIndex;
                _resourceDefs[i] = resourceDef;
                _resourceNameToIndex.Add(resourceDef.cachedName, newIndex);
            }

            resourceDefs = new ReadOnlyCollection<ResourceDef>(_resourceDefs);
            _onAvailable?.Invoke();
            _onAvailable = null;

            void NameCheck(ResourceDef def)
            {
                if (string.IsNullOrEmpty(def.cachedName))
                {
                    Debug.LogWarning("Invalid resourceDef name, using generic name.", def);
                    def.cachedName = "RESOURCEDEF_" + invalidCount;
                    invalidCount++;
                }
            }
        }
    }
}