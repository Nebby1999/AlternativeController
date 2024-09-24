using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AC
{
    [Obsolete]
    [RequireComponent(typeof(SpriteRenderer))]
    public class Mineral : PoolObject, IHarvesteable_OLD
    {
        private SpriteRenderer _sprite;
        public ResourceDef resourceType => _resourceType;
        [SerializeField] private ResourceDef _resourceType;
        private void Awake()
        {
            _sprite = GetComponent<SpriteRenderer>();
        }
        private void OnEnable()
        {
            _sprite.color = resourceType.resourceColor;
        }

        public void InitializeFromPool(ResourceIndex resourceIndex, GameObjectPool poolThatsInstantiating)
        {
            ResourceDef resourceType = ResourceCatalog.GetResourceDef(resourceIndex);
            _resourceType = resourceType; ;
            _sprite.color = resourceType.resourceColor;
            _pool = poolThatsInstantiating;
        }
        public void Harvest(int amount)
        {
            this.gameObject.SetActive(false);
        }
    }
}
