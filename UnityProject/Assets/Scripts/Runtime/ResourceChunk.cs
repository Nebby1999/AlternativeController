using UnityEngine;

namespace AC
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class ResourceChunk : MonoBehaviour
    {
        public ResourceDef resourceDef;
        public float resourceValue;

        new public Rigidbody2D rigidbody2D { get; private set; }
        private SpriteRenderer[] _renderers;

        private void Awake()
        {
            rigidbody2D = GetComponent<Rigidbody2D>();
            _renderers = GetComponentsInChildren<SpriteRenderer>();
        }

        private void Start()
        {
            foreach(var renderer in _renderers)
            {
                renderer.color = resourceDef.resourceColor;
            }
        }
    }
}