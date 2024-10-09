using UnityEngine;

namespace AC
{
    /// <summary>
    /// Representa un Chunk de un Recurso
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public class ResourceChunk : MonoBehaviour
    {
        [Tooltip("El tipo de recurso del Chunk")]
        public ResourceDef resourceDef;
        [Tooltip("El valor del Recurso")]
        public float resourceValue;

        /// <summary>
        /// El rigidbody asociado a este recurso.
        /// </summary>
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