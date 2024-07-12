using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AC
{

    public class ResourcesManager : MonoBehaviour
    {
        private float[] resources;

        private void Awake()
        {
            resources = new float[ResourceCatalog.resourceCount];
        }
        public bool LoadResource(ResourceDef resourceDef, float amount) => LoadResource(resourceDef ? resourceDef.resourceIndex : ResourceIndex.None, amount);

        public bool LoadResource(ResourceIndex resourceIndex, float amount)
        {
            if (resourceIndex == ResourceIndex.None)
                return false;

            int index = (int)resourceIndex;
            resources[index] += amount;
            return true;
        }

        public bool UnloadResource(ResourceDef resourceDef, float amount) => UnloadResource(resourceDef ? resourceDef.resourceIndex : ResourceIndex.None, amount);

        public bool UnloadResource(ResourceIndex resourceIndex, float amount)
        {
            if (resourceIndex == ResourceIndex.None)
                return false;

            int index = (int)resourceIndex;
            resources[index] -= amount;

            return true;
        }
    }
}