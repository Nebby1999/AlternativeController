using Nebula;
using System;
using UnityEngine;

namespace AC
{
    /// <summary>
    /// Componente que representa la "Vida" de un objeto.
    /// </summary>
    public class HealthComponent : MonoBehaviour
    {
        [SerializeField, Tooltip("Valor usado como vida maxima cuando el GameObject no tiene un componente que implementa IHealthComponentInfoProvider.")] private float _defaultMaxHealth;
        [SerializeField, Tooltip("Valor usado como armadura cuando el GameObject no tiene un componente que implementa IHealthComponentInfoProvider.")] private int _defaultArmor;

        private IHealthComponentInfoProvider _infoProvider;

        /// <summary>
        /// La cantidad de vida maxima que tiene este objeto
        /// </summary>
        public float maxHealth => _infoProvider?.maxHp ?? _defaultMaxHealth;

        /// <summary>
        /// La cantidad de armadura que tiene este objeto.
        /// </summary>
        public int armor => _infoProvider?.armor ?? _defaultArmor;

        /// <summary>
        /// La vida actual de este objeto
        /// </summary>
        public float currentHealth { get; private set; }

        /// <summary>
        /// Revisa si el objeto esta vivo o no
        /// </summary>
        public bool isAlive => currentHealth > 0;

        /// <summary>
        /// Causa daño a un objeto.
        /// </summary>
        /// <param name="damageInfo">La informacion de daño</param>
        public void TakeDamage(DamageInfo damageInfo)
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
                //Esto talves se tenga que cambiar.
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
                return 2 - 100 / (100 - armor); //Armadura negativa aumenta el daño recibido hasta un 100%. la escala es hyperbolica
            }
            else
            {
                return 100 / (100 + armor); //Armadura positiva disminuye el daño recibido hasta un 100%. La escala es hyperbolica.
            }
        }
    }

    /// <summary>
    /// La cantidad de vida maxima que tiene este objeto
    /// </summary>
    public interface IHealthComponentInfoProvider
    {
        /// <summary>
        /// La vida maxima de este objeto
        /// </summary>
        public float maxHp { get; }

        /// <summary>
        /// La cantidad de armadura que tiene este objeto.
        /// </summary>
        public int armor { get; }
    }
}