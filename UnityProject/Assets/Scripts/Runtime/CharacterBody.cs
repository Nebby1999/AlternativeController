using System;
using UnityEngine;

namespace AC
{
    public class CharacterBody : MonoBehaviour, IHealthComponentInfoProvider
    {
        public float maxHp { get; private set; }
        [SerializeField] private float _baseHp;

        public float movementSpeed { get; private set; }
        [SerializeField] private float _baseMovementSpeed;

        public float attackSpeed { get; private set; }
        [SerializeField] private float _baseAttackSpeed;

        public float damage { get; private set; }
        [SerializeField] private float _baseDamage;

        public int armor { get; private set; }
        [SerializeField] private int _baseArmor;

        public HealthComponent healthComponent { get; private set; }
        public InputBank inputBank { get; private set; }
        private IBodyStatModifier[] _statModifiers = Array.Empty<IBodyStatModifier>();
        private bool _statsDirty;

        private void Awake()
        {
            healthComponent = GetComponent<HealthComponent>();
            inputBank = GetComponent<InputBank>();
            _statModifiers = GetComponents<IBodyStatModifier>();
        }

        private void Start()
        {
            RecalculateStats();
        }

        [ContextMenu("Recalculate Stats")]
        [RuntimeInspectorNamespace.RuntimeInspectorButton("Recalculate Stats", false, RuntimeInspectorNamespace.ButtonVisibility.InitializedObjects)]
        private void RecalculateStats()
        {
            var args = new StatModifierArgs();
            for(int i = 0; i < _statModifiers.Length; i++)
            {
                var modifier = _statModifiers[i];
                modifier.PreStatRecalculation(this);
                modifier.GetStatCoefficients(args, this);
            }

            float baseStat = _baseHp + args.baseHealthAdd;
            float finalStat = baseStat * args.healthMultAdd;
            maxHp = finalStat;

            baseStat = _baseMovementSpeed + args.baseMovementSpeedAdd;
            finalStat = baseStat * args.movementSpeedMultAdd;
            movementSpeed = finalStat;

            baseStat = _baseAttackSpeed + args.baseAttackSpeedAdd;
            finalStat = baseStat * args.attackSpeedMultAdd;
            attackSpeed = finalStat;

            baseStat = _baseDamage + args.baseDamageAdd;
            finalStat = baseStat * args.damageMultAdd;
            damage = finalStat;

            baseStat = _baseArmor + args.baseArmorAdd;
            finalStat = baseStat * args.armorMultAdd;
            damage = finalStat;

            for(int i = 0; i < _statModifiers.Length; i++)
            {
                _statModifiers[i].PostStatRecalculation(this);
            }

            Debug.Log($"Final Stats for {this}:\n" +
                $"maxHP={maxHp}\n" +
                $"movementSpeed={movementSpeed}\n" +
                $"attackSpeed={attackSpeed}\n" +
                $"damage={damage}\n" +
                $"armor={armor}");
        }

        public void SetStatsDirty() => _statsDirty = true;

        private void FixedUpdate()
        {
            if(_statsDirty)
            {
                _statsDirty = false;
                RecalculateStats();
            }
        }
    }

    public interface IBodyStatModifier
    {
        public void PreStatRecalculation(CharacterBody body);

        public void PostStatRecalculation(CharacterBody body);

        public void GetStatCoefficients(StatModifierArgs args, CharacterBody body);
    }

    public class StatModifierArgs
    {
        public float baseHealthAdd = 0;
        public float healthMultAdd = 1;

        public float baseMovementSpeedAdd = 0;
        public float movementSpeedMultAdd = 1;

        public float baseAttackSpeedAdd = 0;
        public float attackSpeedMultAdd = 1;

        public float baseDamageAdd = 0;
        public float damageMultAdd = 1;

        public int baseArmorAdd = 0;
        public float armorMultAdd = 1;
    }
}