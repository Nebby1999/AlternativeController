using Nebula;
using System.Collections.Generic;
using UnityEngine;

namespace AC
{
    [RequireComponent(typeof(Collider2D))]
    public class HurtBox : MonoBehaviour
    {
        public static readonly List<HurtBox> bullseyeHurtboxInstances = new List<HurtBox>();
        public new Collider2D collider { get; private set; }

        public HealthComponent healthComponent => _healthComponent;
        [SerializeField] private HealthComponent _healthComponent;
        public bool isBullseye => _isBullseye;
        [SerializeField] private bool _isBullseye;

        private Rigidbody2D _rigidbody;
        private void Awake()
        {
            collider = GetComponent<Collider2D>();
            collider.isTrigger = false;
            _rigidbody = this.EnsureComponent<Rigidbody2D>();
            _rigidbody.isKinematic = true;
            _rigidbody.hideFlags = HideFlags.NotEditable;
        }

        private void OnEnable()
        {
            if (_isBullseye)
                bullseyeHurtboxInstances.Add(this);
        }

        private void OnDisable()
        {
            if (_isBullseye)
                bullseyeHurtboxInstances.Remove(this);
        }
    }
}