using Nebula;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AC
{
    /// <summary>
    /// Representa una Base dentro de la escena, el cual consume recursos y el <see cref="HeadQuarters"/> junto con los jugadores en <see cref="Vehicle"/> deben mantener vivos dandoles recursos.
    /// </summary>
    [RequireComponent(typeof(ResourcesManager))]
    public class Base : MonoBehaviour
    {
        [Tooltip("El nombre de la base, se usa en la UI")]
        public string baseName;
        [Tooltip("El tipo de recurso que la base ocupa")]
        [SerializeField] private ResourceDef _type;
        [Tooltip("La base empieza con esta cantidad de recursos al comienzo")]
        [SerializeField] private float _startingResources;
        [Tooltip("La base consume esta cantidad de recursos por segundo")]
        [SerializeField] private float _resourceLossPerSecond;
        private SpriteRenderer _sprite;

        /// <summary>
        /// El <see cref="ResourcesManager"/> relacionado con esta base.
        /// </summary>
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

        /// <summary>
        /// <inheritdoc cref="ResourcesManager.LoadResource(ResourceDef, float)"/>
        /// </summary>
        public void TryLoadResource(ResourceDef resourceDef, float amount) => TryLoadResource(resourceDef.resourceIndex, amount);

        /// <summary>
        /// <inheritdoc cref="ResourcesManager.LoadResource(ResourceIndex, float)"/>
        /// </summary>
        public void TryLoadResource(ResourceIndex resourceIndex, float amount)
        {
            _resources.LoadResource(resourceIndex, amount);
        }
    }
}
