using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AC
{

    [RequireComponent(typeof(SpriteRenderer))]
    public class MineralOre_OLD : MonoBehaviour, IHarvesteable_OLD
    {
        private SpriteRenderer _sprite;

        public ResourceDef resourceType => _resourceType;
        [SerializeField] ResourceDef _resourceType;
        [SerializeField, Range(100, 1000)] private int _resources = 100;
        private void Awake()
        {
            _sprite = GetComponent<SpriteRenderer>();
        }
        private void OnEnable()
        {
            _sprite.color = _resourceType.resourceColor;
        }


        public void Harvest(int amount)
        {
            _resources -= amount;
            if(_resources <= 0) this.gameObject.SetActive(false);
        }
    }
}
