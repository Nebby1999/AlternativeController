using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AC
{
    [Serializable]
    public class Cargo
    {
        private readonly int _maxCapacity;
        private int[] _mineralCount;
        public Queue<ResourceIndex> resourceCollectionOrder { get; private set; }
        public ResourceIndex lastUnloadedResource { get; private set; }
        public int totalCargoHeld => resourceCollectionOrder.Count;
        public bool isFull => totalCargoHeld >= _maxCapacity;
        public bool isEmpty => totalCargoHeld < 1;

        public Cargo(int totalCapacity)
        {
            resourceCollectionOrder = new Queue<ResourceIndex>(totalCapacity);
            _mineralCount = new int[ResourceCatalog.resourceCount];
            _maxCapacity = totalCapacity;
        }

        public bool LoadResource(ResourceDef resource, int amount)
        {
            if (isFull)
                return false;

            var resourceIndex = resource.resourceIndex;
            var indexAsInt = (int)resourceIndex;
            _mineralCount[indexAsInt] += amount;
            for (int i = 0; i < amount; i++)
            {
                resourceCollectionOrder.Enqueue(resourceIndex);
            }
            return true;
        }

        public bool UnloadResource(int amount)
        {
            if (isEmpty)
                return false;

            for (int i = 0; i < amount; i++)
            {
                lastUnloadedResource = resourceCollectionOrder.Dequeue();
                _mineralCount[(int)lastUnloadedResource]--;
            }
            return true;
        }

        public int GetResourceCount(ResourceDef def) => def ? GetResourceCount(def.resourceIndex) : 0;
        public int GetResourceCount(ResourceIndex index) => _mineralCount[(int)index];
    }
}
