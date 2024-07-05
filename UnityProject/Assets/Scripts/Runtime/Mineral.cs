using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AC
{

    public enum MineralType
    {
        Black,
        Red
    }
    [RequireComponent(typeof(SpriteRenderer))]
    public class Mineral : PoolObject, IHarvesteable
    {
        private SpriteRenderer _sprite;
        public MineralType Type => _type;
        [SerializeField] private MineralType _type;
        private void Awake()
        {
            _sprite = GetComponent<SpriteRenderer>();
        }
        private void OnEnable()
        {
            _sprite.color = _type == MineralType.Black ? Color.black : Color.red;
        }
        public void Initialize(MineralType type, GameObjectPool pool)
        {
            _type = type;
            _pool = pool;
            _sprite.color = _type == MineralType.Black ? Color.black : Color.red;
        }
        public void Harvest(int amount)
        {
            this.gameObject.SetActive(false);
        }
    }
}
