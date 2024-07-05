using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AC
{
    [RequireComponent(typeof(ResourcesManager))]
    [RequireComponent(typeof(SpriteRenderer))]
    public class Base : MonoBehaviour
    {
        [SerializeField] private MineralType _type;
        private SpriteRenderer _sprite;
        private ResourcesManager _resources;
        private void Awake()
        {
            _sprite = GetComponent<SpriteRenderer>();
            _resources = GetComponent<ResourcesManager>();
        }
        private void OnEnable()
        {
            _sprite.color = _type == MineralType.Black ? Color.black : Color.red;
        }
        public void TryLoadMineral(MineralType mineral, float amount)
        {
            _resources.LoadMaterial(mineral, amount);
        }
    }
}
