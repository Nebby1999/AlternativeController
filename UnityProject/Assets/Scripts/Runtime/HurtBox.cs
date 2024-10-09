using Nebula;
using System.Collections.Generic;
using UnityEngine;

namespace AC
{
    /// <summary>
    /// Representa una Hurtbox de un objeto con un collider, la <see cref="HurtBox"/> despues es utilizada en los metodos para dañar objetos en el juego.
    /// </summary>
    [RequireComponent(typeof(Collider2D))]
    public class HurtBox : MonoBehaviour
    {
        /// <summary>
        /// Una lista estatica de todos los <see cref="HurtBox"/> activos que tienen <see cref="isBullseye"/> con valor true
        /// </summary>
        public static readonly List<HurtBox> bullseyeHurtboxInstances = new List<HurtBox>();
        /// <summary>
        /// El collider ligado a este HurtBox
        /// </summary>
        public new Collider2D collider { get; private set; }

        /// <summary>
        /// El componente de vida ligado a este Hurtbox
        /// </summary>
        public HealthComponent healthComponent => _healthComponent;
        [SerializeField, Tooltip("El componente de vida ligado a este Hurtbox")] private HealthComponent _healthComponent;

        /// <summary>
        /// Representa si este HurtBox es un "Blanco". usualmente el mejor hurtbox de un objeto.
        /// </summary>
        public bool isBullseye => _isBullseye;
        [SerializeField] private bool _isBullseye;

        private Rigidbody2D _rigidbody;
        private void Awake()
        {
            collider = GetComponent<Collider2D>();
            collider.isTrigger = false;
            _rigidbody = this.EnsureComponent<Rigidbody2D>(); //Tenemos que asegurarnos que esta hurtbox sea un rigidbody para raycasts
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