using System;
using UnityEngine;

namespace AC
{
    /// <summary>
    /// Un CharacterBody es un objeto en escena que tiene "Vida" y representa un personaje dentro de este que tiene estados y sistemas.
    /// <para></para>
    /// Los cuerpos no son nada mas que titeres y carecen de una inteligencia artificial propia. El cuerpo es controlado por un <see cref="CharacterMaster"/> que usualmente hace aparecer al cuerpo en si.
    /// <br></br>
    /// Puedes pensar que el cuerpo es como un "Saco de Carne" de un personaje, mientras que el <see cref="CharacterMaster"/> es el Cerebro del personaje que le dice como actuar o moverse.
    /// </summary>
    public class CharacterBody : MonoBehaviour, IHealthComponentInfoProvider
    {
        public float maxHp { get; private set; }
        [Tooltip("La vida base del personaje")]
        [SerializeField] private float _baseHp;

        public float movementSpeed { get; private set; }
        [Tooltip("La velocidad de movimiento base del personaje")]
        [SerializeField] private float _baseMovementSpeed;

        public float attackSpeed { get; private set; }
        [Tooltip("La velocidad de ataque base del personaje")]
        [SerializeField] private float _baseAttackSpeed;

        public float damage { get; private set; }
        [Tooltip("El daño base del personaje")]
        [SerializeField] private float _baseDamage;

        public int armor { get; private set; }
        [Tooltip("La armadura base del personaje, mientras mas alta es la armadura mas daño negara al recibir daño")]
        [SerializeField] private int _baseArmor;

        /// <summary>
        /// El componente de vida acoplado a este cuerpo
        /// </summary>
        public HealthComponent healthComponent { get; private set; }

        /// <summary>
        /// El <see cref="InputBank"/> asociado a este cuerpo.
        /// </summary>
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
            var args = new StatModifierArgs(); //Creamos un modificador de stats, lo cual tendra la metadata necesaria para recalcular los stats.
            for(int i = 0; i < _statModifiers.Length; i++)
            {
                var modifier = _statModifiers[i];
                modifier.PreStatRecalculation(this); //Llamamos los callbacks
                modifier.GetStatCoefficients(args, this);
            }

            float baseStat = _baseHp + args.baseHealthAdd; //Modificamos los stats
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
                _statModifiers[i].PostStatRecalculation(this); //Llamamos el callback de post recalculacion
            }

            Debug.Log($"Final Stats for {this}:\n" +
                $"maxHP={maxHp}\n" +
                $"movementSpeed={movementSpeed}\n" +
                $"attackSpeed={attackSpeed}\n" +
                $"damage={damage}\n" +
                $"armor={armor}");
        }

        /// <summary>
        /// Indica que los Stats de este cuerpo estan sucios y deberian ser recalculados
        /// </summary>
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