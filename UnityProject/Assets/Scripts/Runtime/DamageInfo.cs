using UnityEngine;

namespace AC
{
    /// <summary>
    /// Representa informacion basica para que un <see cref="HealthComponent"/> reciba daño usando <see cref="HealthComponent.TakeDamage(DamageInfo)"/>
    /// </summary>
    public class DamageInfo
    {
        /// <summary>
        /// El objeto que esta atacando
        /// </summary>
        public GameObject attackerObject;
        /// <summary>
        /// El cuerpo que esta atacando
        /// </summary>
        public CharacterBody attackerBody;

        /// <summary>
        /// La cantidad de daño que la victima deberia recibir
        /// </summary>
        public float damage;
        /// <summary>
        /// Si el daño puede stunnear a la victima.
        /// </summary>
        public bool isStunning;
    }
}