using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AC
{

    public class ShootController : MonoBehaviour
    {
        [SerializeField] private GameObjectPool _bulletPool;
        [SerializeField] private Transform _pivot;
        [SerializeField] private float _shootingDelay = 1f;
        [SerializeField] private float _cooldown;
        private BulletFactory _bulletFactory;
        private void Awake()
        {
            _bulletFactory = new BulletFactory(_bulletPool);
        }
        private void Update()
        {
            if(_cooldown > 0) _cooldown -= Time.deltaTime;
            _cooldown = Mathf.Clamp(_cooldown, 0f, _shootingDelay);
        }
        public void TryShooting(bool input)
        {
            print($"{!input || _cooldown > 0f}");
            if(!input || _cooldown > 0f) return;
            _bulletFactory.CreateBullet(_pivot.position, _pivot.rotation);
            _cooldown = _shootingDelay;
            Debug.Log("Shooting bullet");
        }
    }
}
