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
        public int[] mineralCount;
        public Queue<ResourceIndex> resourceCollectionOrder { get; private set; }
        public ResourceIndex lastUnloadedResource { get; private set; }
        public int totalCargoHeld => resourceCollectionOrder.Count;
        public bool isFull => totalCargoHeld >= _maxCapacity;
        public bool isEmpty => totalCargoHeld < 1;

        public Cargo(int totalCapacity)
        {
            resourceCollectionOrder = new Queue<ResourceIndex>(totalCapacity);
            _maxCapacity = totalCapacity;
        }

        public bool LoadResource(ResourceDef resource, int amount)
        {
            if (isFull)
                return false;

            var resourceIndex = resource.resourceIndex;
            var indexAsInt = (int)resourceIndex;
            mineralCount[indexAsInt] += amount;
            for(int i = 0; i < amount; i++)
            {
                resourceCollectionOrder.Enqueue(resourceIndex);
            }
            return true;
        }

        public bool UnloadResource(int amount)
        {
            if (isEmpty)
                return false;

            for(int i = 0; i < amount; i++)
            {
                lastUnloadedResource = resourceCollectionOrder.Dequeue();
                mineralCount[(int)lastUnloadedResource]--;
            }
            return true;
        }
    }
}
