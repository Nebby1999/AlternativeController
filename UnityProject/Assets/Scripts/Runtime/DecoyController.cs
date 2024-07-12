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

        public void TryDecoy(bool canDecoy, ResourceIndex resource)
        {
            if (!canDecoy)
                return;

            _mineralFactory.CreateMineral(resource, _transform.position, _transform.rotation);
        }
    }
}