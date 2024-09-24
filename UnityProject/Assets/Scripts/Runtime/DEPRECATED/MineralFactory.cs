using System;
using UnityEngine;

namespace AC
{

    [Obsolete]
    public class MineralFactory
    {
        private readonly GameObjectPool _mineralPool;

        public MineralFactory(GameObjectPool mineralPool)
        {
            _mineralPool = mineralPool;
        }

        public Mineral CreateMineral(ResourceIndex resourceIndex, Vector3 position, Quaternion rotation)
        {
            GameObject mineralGO = _mineralPool.SpawnObject(position, rotation);
            Mineral mineral = mineralGO.GetComponent<Mineral>();
            mineral.InitializeFromPool(resourceIndex, _mineralPool);
            return mineral;
        }
    }
}