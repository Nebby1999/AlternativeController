using Nebula;
using UnityEngine;

namespace AC
{
    /// <summary>
    /// Representa un deposito de minerales.
    /// </summary>
    public class ResourceOreDeposit : MonoBehaviour, IHarvestable
    {
        [Tooltip("La cantidad de recursos guardados en este deposito.")]
        [SerializeField] private int _resourcesCount;

        /// <summary>
        /// Cuantos recursos le quedan al deposito.
        /// </summary>
        public int resourceRemaining { get; private set; }

        /// <summary>
        /// El tipo de recurso en este deposito
        /// </summary>
        public ResourceDef resourceType => _resourceType;
        [Tooltip("El tipo de recurso de este deposito")]
        [SerializeField] private ResourceDef _resourceType;

        [Tooltip("Renderer para el deposito.")]
        [SerializeField] private SpriteRenderer _spriteRenderer;
        private Vector3 _originalSize;

        private void Awake()
        {
            resourceRemaining = _resourcesCount;
        }

        private void Start()
        {
            if (_spriteRenderer)
                _spriteRenderer.color = resourceType.resourceColor;

            _originalSize = transform.localScale;
        }

        private void OnEnable()
        {
            InstanceTracker.Add(this);
        }

        private void OnDisable()
        {
            InstanceTracker.Remove(this);
        }

        private void Update()
        {
            transform.localScale = Vector3.Lerp(Vector3.zero, _originalSize, (float)resourceRemaining / (float)_resourcesCount);
        }

        public void Harvest(int amount)
        {
            resourceRemaining -= amount;
            if(resourceRemaining <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}