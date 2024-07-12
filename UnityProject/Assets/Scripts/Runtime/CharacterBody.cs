using UnityEngine;

namespace AC
{
    public class CharacterBody : MonoBehaviour, IMaxHealthProvider
    {
        public float maxHp => _hp;
        [SerializeField] private float _hp;

        public float movementSpeed => _movementSpeed;
        [SerializeField] private float _movementSpeed;

        public float attackSpeed => _attackSpeed;
        [SerializeField] private float _attackSpeed;

        public float damage => _damage;
        [SerializeField] private float _damage;

        public HealthComponent healthComponent { get; private set; }
        public InputBank inputBank { get; private set; }

        private void Awake()
        {
            healthComponent = GetComponent<HealthComponent>();
            inputBank = GetComponent<InputBank>();
        }
    }
}