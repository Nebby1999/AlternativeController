using Nebula;
using UnityEngine;

namespace AC
{
    public class ResourceOreDeposit : MonoBehaviour, IHarvestable
    {
        [SerializeField] private int _resourcesCount;
        public int resourceRemaining { get; private set; }
        public ResourceDef resourceType => _resourceType;
        [SerializeField] private ResourceDef _resourceType;
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