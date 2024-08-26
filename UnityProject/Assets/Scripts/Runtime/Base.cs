using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AC
{
    [RequireComponent(typeof(ResourcesManager))]
    public class Base : MonoBehaviour
    {
        [SerializeField] private ResourceDef _type;
        [SerializeField] private float resourceLossPerSecond;
        private SpriteRenderer _sprite;
        private ResourcesManager _resources;

        private Vector3 _origScale;
        private void Awake()
        {
            _sprite = GetComponentInChildren<SpriteRenderer>();
            _resources = GetComponent<ResourcesManager>();
        }

        private void Start()
        {
            _origScale = transform.localScale;
        }

        private void OnEnable()
        {
            _sprite.color = _type.resourceColor;
        }

        private void Update()
        {
            transform.localScale = _origScale * (1 + _resources.totalResourcesCont / 5);
        }

        private void FixedUpdate()
        {
            _resources.UnloadResource(_type, resourceLossPerSecond * Time.fixedDeltaTime);
        }

        public void TryLoadMineral(ResourceDef resourceDef, float amount) => TryLoadMineral(resourceDef.resourceIndex, amount);
        public void TryLoadMineral(ResourceIndex resourceIndex, float amount)
        {
            _resources.LoadResource(resourceIndex, amount);
        }
    }
}
