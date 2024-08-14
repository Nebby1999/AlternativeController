using System;
using UnityEngine;
using UnityEngine.Events;

namespace AC
{
    public class OnTrigger2DEventBehaviour : MonoBehaviour
    {

        public TriggerEvent onEnter;
        public TriggerEvent onExit;
        public TriggerEvent onStay;

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

        [Serializable]
        public class TriggerEvent : UnityEvent<Collider2D> { }
    }
}