using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AC
{
    [RequireComponent(typeof(ResourcesManager))]
    [RequireComponent(typeof(SpriteRenderer))]
    public class Base : MonoBehaviour
    {
        [SerializeField] private ResourceDef _type;
        private SpriteRenderer _sprite;
        private ResourcesManager _resources;
        private void Awake()
        {
            _sprite = GetComponent<SpriteRenderer>();
            _resources = GetComponent<ResourcesManager>();
        }
        private void OnEnable()
        {
            _sprite.color = _type.resourceColor;
        }

        public void TryLoadMineral(ResourceDef resourceDef, float amount) => TryLoadMineral(resourceDef.resourceIndex, amount);
        public void TryLoadMineral(ResourceIndex resourceIndex, float amount)
        {
            _resources.LoadResource(resourceIndex, amount);
        }
    }
}
