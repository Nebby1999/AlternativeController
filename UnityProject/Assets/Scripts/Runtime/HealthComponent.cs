using UnityEngine;

namespace AC
{
    public class HealthComponent : MonoBehaviour
    {
        [SerializeField] private float _defaultMaxHealth;
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

        private void Awake()
        {
            _maxHealthProvider = GetComponent<IMaxHealthProvider>();
        }
    }

    public interface IMaxHealthProvider
    {
        public float maxHp { get; }
    }
}