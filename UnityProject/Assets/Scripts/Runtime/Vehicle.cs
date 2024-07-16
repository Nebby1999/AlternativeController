using System;
using UnityEngine;

namespace AC
{
    [RequireComponent(typeof(CharacterBody))]
    public class Vehicle : MonoBehaviour, IBodyStatModifier
    {
        public float heat { get; private set; }
        public float maxHeat => _maxHeat;
        [SerializeField]private float _maxHeat;
        [NonSerialized]private Cargo _cargo;
        public CharacterBody characterBody { get; private set; }
        public bool isInCombatMode { get; private set; }

        private void Awake()
        {
            characterBody = GetComponent<CharacterBody>();
            _cargo ??= new Cargo(10);
            _maxHeat = _maxHeat == 0 ? 50 : _maxHeat;
        }

        public void PreStatRecalculation(CharacterBody body) { }

        public void PostStatRecalculation(CharacterBody body) { }

        public void GetStatCoefficients(StatModifierArgs args, CharacterBody body)
        {
            if(!isInCombatMode)
            {
                args.movementSpeedMultAdd -= 0.2f;
            }
        }
    }
}