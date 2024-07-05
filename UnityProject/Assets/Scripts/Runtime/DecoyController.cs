using UnityEngine;

namespace AC
{

    public class DecoyController
    {
        private readonly MineralFactory _mineralFactory;
        private readonly Transform _transform;
        public DecoyController(GameObjectPool pool, Transform transform)
        {
            _mineralFactory = new MineralFactory(pool);
            _transform = transform;
        }
        public void TryDecoy(bool canDecoy, MineralType mineral)
        {
            if(!canDecoy) return;
            _mineralFactory.CreateMineral(mineral, _transform.position, _transform.rotation);
        }
    }
}