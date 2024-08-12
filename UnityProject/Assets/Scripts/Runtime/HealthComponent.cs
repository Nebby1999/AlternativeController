using System;
using UnityEngine;

namespace AC
{
    public class HealthComponent : MonoBehaviour
    {
        [SerializeField, Tooltip("Value that's used as max health when the game object doesnt have a component that implements IMaxHealthProvider")] private float _defaultMaxHealth;
        private IMaxHealthProvider _maxHealthProvider;

        public float maxHealth
        {
            get
            {
                return _maxHealthProvider?.maxHp ?? _defaultMaxHealth;
            }
        }

        public float currentHealth { get; private set; }

        public bool isAlive => currentHealth > 0;

        internal void TakeDamage(DamageInfo damageInfo)
        {
            var dmg = damageInfo.damage;
            Debug.Log(this + " Takes " + dmg + " Damage!");
            currentHealth -= dmg;

            if(damageInfo.isStunning)
            {
                //Stun
            }
        }

        private void FixedUpdate()
        {
            if (!isAlive)
            {
                Destroy(gameObject);
            }
        }
        private void Awake()
        {
            _maxHealthProvider = GetComponent<IMaxHealthProvider>();
        }

        private void Start()
        {
            currentHealth = _maxHealthProvider?.maxHp ?? _defaultMaxHealth;
        }
    }

    public interface IMaxHealthProvider
    {
        public float maxHp { get; }
    }
}