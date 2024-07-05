using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AC
{
    /// <summary>
    /// Catalog which manages and contains all the ResourceDefs of the game
    /// </summary>
    public static class ResourceCatalog
    {
        /// <summary>
        /// Obtain the total amount of ResourceDefs currently stored in the catalog.
        /// </summary>
        public static int resourceCount => _resourceDefs.Length;
        private static ResourceDef[] _resourceDefs = Array.Empty<ResourceDef>();
        //A dictionary to store the ResourceIndex and to be able to find them using just the name of the resource.
        private static Dictionary<string, ResourceIndex> _resourceNameToIndex = new Dictionary<string, ResourceIndex>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Event which can be subscribed so code can run once the Catalog becomes Available.
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
        /// returns wether or not the catalog has been initialized and it's currently available.
        /// </summary>
        public static bool initialized { get; private set; }

        /// <summary>
        /// Returns the ResourceDef associated with the specified index. An index can be obtained at the runtime level using <see cref="FindResource(string)"/>
        /// </summary>
        /// <param name="index">The index to use</param>
        /// <returns>The associated ResourceDef, if ResourceIndex is None, out of range, or the catalog hasn't initialized, it returns null.</returns>
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
        /// Finds the Index for a specified resource. The resource's ResourceDef then can be obtained using <see cref="GetResourceDef(ResourceIndex)"/>
        /// </summary>
        /// <param name="resourceDefName">The name of the resource</param>
        /// <returns>A valid resource index, or <see cref="ResourceIndex.None"/> if no match was found OR if the catalog is not initialized</returns>
        public static ResourceIndex FindResource(string resourceDefName)
        {
            if (!initialized)
                return ResourceIndex.None;

            return _resourceNameToIndex.TryGetValue(resourceDefName, out var index) ? index : ResourceIndex.None;
        }

        /// <summary>
        /// Initializes the ResourceCatalog.
        /// </summary>
        internal static void Initialize()
        {
            if (initialized)
                return;

            initialized = true;

            //Load all resources
            ResourceDef[] resources = LoadResourceDefs();
            _resourceDefs = new ResourceDef[resources.Length];

            int invalidCount = 0;
            for(int i = 0; i < resources.Length; i++)
            {
                var resourceDef = resources[i];

                if(string.IsNullOrEmpty(resourceDef.cachedName))
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

            _onAvailable?.Invoke();
            _onAvailable = null;
        }

        //TODO: Decide wether to use Resources folder or Addressables.
        private static ResourceDef[] LoadResourceDefs() => new ResourceDef[0];
    }
}