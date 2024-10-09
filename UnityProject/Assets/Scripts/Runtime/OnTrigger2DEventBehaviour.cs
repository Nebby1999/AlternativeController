using System;
using UnityEngine;
using UnityEngine.Events;

namespace AC
{
    /// <summary>
    /// Un behaviour el cual llama eventos relacionados a <see cref="Collider2D"/>, como "OnTriggerEnter/Exit/Stay2D"
    /// </summary>
    [RequireComponent(typeof(Collider2D))]
    public class OnTrigger2DEventBehaviour : MonoBehaviour
    {
        [Tooltip("Evento ejecutado cuando un collider entra a nuestro trigger.")]
        public TriggerEvent onEnter;
        [Tooltip("Evento ejecutado cuando un collider sale de nuestro trigger")]
        public TriggerEvent onExit;
        [Tooltip("Evento ejecutado por cada frame que un collider este en nuestro trigger.")]
        public TriggerEvent onStay;

        /// <summary>
        /// El collider asociado con este behaviour
        /// </summary>
        new public Collider2D collider2D { get; private set; }

        private void Awake()
        {
            collider2D = GetComponent<Collider2D>();
        }

        private void Start()
        {
            collider2D.isTrigger = true;
        }


        private void OnTriggerEnter2D(Collider2D collision)
        {
            onEnter?.Invoke(collision);
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            onExit?.Invoke(collision);
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            onStay?.Invoke(collision);
        }

        /// <summary>
        /// Representa un <see cref="UnityEvent{T0}"/> donde el primer Argumento es un <see cref="Collider2D"/>
        /// </summary>
        [Serializable]
        public class TriggerEvent : UnityEvent<Collider2D> { }
    }
}