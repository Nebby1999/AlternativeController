using Nebula;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AC
{
    [RequireComponent(typeof(ResourcesManager))]
    public class Base : MonoBehaviour
    {
        [Tooltip("The name of this base, used for UI.")]
        public string baseName;
        [SerializeField] private ResourceDef _type;
        [Tooltip("The base starts with this amount of resources")]
        [SerializeField] private float _startingResources;
        [Tooltip("The base consumes this amount of resources per second")]
        [SerializeField] private float _resourceLossPerSecond;
        private SpriteRenderer _sprite;

        public ResourcesManager resourcesManager => _resources;
        private ResourcesManager _resources;

        private Vector3 _origScale;
        private void Awake()
        {
            _sprite = GetComponentInChildren<SpriteRenderer>();
            _resources = GetComponent<ResourcesManager>();
        }

        private void Start()
        {
            _resources.LoadResource(_type, _startingResources);
            _origScale = transform.localScale;
        }

        private void OnEnable()
        {
            InstanceTracker.Add(this);
            _sprite.color = _type.resourceColor;
        }

        private void OnDisable()
        {
            InstanceTracker.Remove(this);
        }

        private void Update()
        {
            float t = NebulaMath.Remap(_resources.totalResourcesCont, 0, _startingResources * 2, 0, 1);
            transform.localScale = Vector3.Lerp(Vector3.zero, _origScale, t);
        }

        private void FixedUpdate()
        {
            _resources.UnloadResource(_type, _resourceLossPerSecond * Time.fixedDeltaTime);
        }

        public void TryLoadMineral(ResourceDef resourceDef, float amount) => TryLoadMineral(resourceDef.resourceIndex, amount);
        public void TryLoadMineral(ResourceIndex resourceIndex, float amount)
        {
            _resources.LoadResource(resourceIndex, amount);
        }
    }
}
