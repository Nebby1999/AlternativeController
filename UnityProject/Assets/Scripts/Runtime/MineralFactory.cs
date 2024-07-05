using UnityEngine;

namespace AC
{

    public class MineralFactory
    {
        private readonly GameObjectPool _mineralPool;

        public MineralFactory(GameObjectPool mineralPool)
        {
            _mineralPool = mineralPool;
        }

        public Mineral CreateMineral(MineralType type, Vector3 position, Quaternion rotation)
        {
            GameObject mineralGO = _mineralPool.SpawnObject(position, rotation);
            Mineral mineral = mineralGO.GetComponent<Mineral>();
            mineral.Initialize(type, _mineralPool);
            return mineral;
        }
    }
}