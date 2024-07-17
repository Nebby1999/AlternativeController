using EntityStates;
using System;
using System.Reflection.Emit;
using UnityEngine;

namespace AC
{
    [RequireComponent(typeof(CharacterBody))]
    public class Vehicle : MonoBehaviour, IBodyStatModifier
    {
        public float heat { get; private set; }
        public float maxHeat => _maxHeat;
        public bool isOverHeated { get; private set; } = false;
        [SerializeField] private float _maxHeat;
        [SerializeField] private EntityStateMachine[] _stateMachinesToStun = Array.Empty<EntityStateMachine>();
        [NonSerialized]private Cargo _cargo;
        public CharacterBody characterBody { get; private set; }
        public bool isInCombatMode { get; private set; }

        private void Awake()
        {
            characterBody = GetComponent<CharacterBody>();
            _cargo ??= new Cargo(10);
            _maxHeat = _maxHeat == 0 ? 50 : _maxHeat;
        }

        public void AddHeat(float heatAmount)
        {
            heat += heatAmount;
        }

        public void RemoveHeat(float heatAmount)
        {
            heat -= heatAmount;
        }

        private void FixedUpdate()
        {
            if(isOverHeated)
            {
                if(heat <= 0)
                {
                    isOverHeated = false;
                    heat = 0;
                    return;
                }
                RemoveHeat((maxHeat / HeatStunState.stunDuration) * Time.fixedDeltaTime);
                return;
            }
            if(heat > maxHeat && !isOverHeated)
            {
                isOverHeated = true;
                foreach(EntityStateMachine stateMachine in _stateMachinesToStun)
                {
                    stateMachine.SetNextState(new HeatStunState());
                }
            }
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

#if UNITY_EDITOR
        [ContextMenu("Populate State Machine Array")]
        private void PopulateStateMachineArray()
        {
            _stateMachinesToStun = GetComponents<EntityStateMachine>();
            UnityEditor.EditorUtility.SetDirty(this);
        }
#endif

#if DEBUG
        public bool printHeatOnScreen = false;

        private void OnGUI()
        {
            if(printHeatOnScreen)
            {
                GUIStyle headStyle = new GUIStyle();
                headStyle.fontSize = 30;
                GUILayout.Label("Heat: " + heat, headStyle);
            }
        }
#endif
    }
}