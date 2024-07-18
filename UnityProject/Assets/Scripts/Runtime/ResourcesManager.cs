using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AC
{

    public class ResourcesManager : MonoBehaviour
    {
        private int[] _resources;

        private void Awake()
        {
            _resources = new int[ResourceCatalog.resourceCount];
        }
        public bool LoadResource(ResourceDef resourceDef, int amount) => LoadResource(resourceDef ? resourceDef.resourceIndex : ResourceIndex.None, amount);

        public bool LoadResource(ResourceIndex resourceIndex, int amount)
        {
            if (resourceIndex == ResourceIndex.None)
                return false;

            int index = (int)resourceIndex;
            _resources[index] += amount;
            return true;
        }

        public bool UnloadResource(ResourceDef resourceDef, int amount) => UnloadResource(resourceDef ? resourceDef.resourceIndex : ResourceIndex.None, amount);

        public bool UnloadResource(ResourceIndex resourceIndex, int amount)
        {
            if (resourceIndex == ResourceIndex.None)
                return false;

            int index = (int)resourceIndex;
            _resources[index] -= amount;

            return true;
        }
    }
}