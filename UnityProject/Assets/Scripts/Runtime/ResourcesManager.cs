using Nebula;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AC
{

    public class ResourcesManager : MonoBehaviour
    {
        public HashSet<ResourceIndex> resourceTypesStored = new HashSet<ResourceIndex>();
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

        public event Action onEmpty;

        private void Awake()
        {
            _resources = new float[ResourceCatalog.resourceCount];
        }

        public float GetResourceCount(ResourceDef resourceDef) => GetResourceCount(resourceDef ? resourceDef.resourceIndex : ResourceIndex.None);

        public float GetResourceCount(ResourceIndex index) => ArrayUtils.GetSafe(ref _resources, (int)index);
        public bool LoadResource(ResourceDef resourceDef, float amount) => LoadResource(resourceDef ? resourceDef.resourceIndex : ResourceIndex.None, amount);

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

        public bool UnloadResource(ResourceDef resourceDef, float amount) => UnloadResource(resourceDef ? resourceDef.resourceIndex : ResourceIndex.None, amount);

        public bool UnloadResource(ResourceIndex resourceIndex, float amount)
        {
            if (resourceIndex == ResourceIndex.None)
                return false;

            int index = (int)resourceIndex;
            _resources[index] -= amount;

            if (_resources[index] < 0) _resources[index] = 0;

            if (resourceTypesStored.Contains(resourceIndex) && _resources[index] == 0)
                resourceTypesStored.Remove(resourceIndex);

            return true;
        }
    }
}