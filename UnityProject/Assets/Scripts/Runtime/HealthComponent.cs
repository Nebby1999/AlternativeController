using Nebula;
using System;
using UnityEngine;

namespace AC
{
    public class HealthComponent : MonoBehaviour
    {
        [SerializeField, Tooltip("Value that's used as max health when the game object doesnt have a component that implements IHealthComponentInfoProvider")] private float _defaultMaxHealth;
        [SerializeField, Tooltip("Value that's used as armor when the game object doesnt have a component that implements IHealthComponentInfoProvider")] private int _defaultArmor;

        private IHealthComponentInfoProvider _infoProvider;

        public float maxHealth => _infoProvider?.maxHp ?? _defaultMaxHealth;

        public int armor => _infoProvider?.armor ?? _defaultArmor;

        public float currentHealth { get; private set; }

        public bool isAlive => currentHealth > 0;

        internal void TakeDamage(DamageInfo damageInfo)
        {
            var dmg = damageInfo.damage;

            dmg *= CalculateDamageMultFromArmor();

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
            _infoProvider = GetComponent<IHealthComponentInfoProvider>();
        }

        private void Start()
        {
            currentHealth = _infoProvider?.maxHp ?? _defaultMaxHealth;
        }

        private float CalculateDamageMultFromArmor()
        {
            if (armor == 0)
                return 1;

            var sign = Mathf.Sign(armor);
            if(sign == -1)
            {
                return 2 - 100 / (100 - armor); //negative armor increases damage taken by up to 100%, scaling is diminishing
            }
            else
            {
                return 100 / (100 + armor); //Positive armor decreases damage taken by up to 100%, scaling is diminishing
            }
        }
    }

    public interface IHealthComponentInfoProvider
    {
        public float maxHp { get; }
        public int armor { get; }
    }
}