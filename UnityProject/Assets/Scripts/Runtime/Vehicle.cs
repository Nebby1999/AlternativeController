using EntityStates;
using System;
using System.Reflection.Emit;
using UnityEngine;

namespace AC
{
    [RequireComponent(typeof(CharacterBody), typeof(SkillManager))]
    public class Vehicle : MonoBehaviour, IBodyStatModifier
    {
        public float heat { get; private set; }
        public float maxHeat => _maxHeat;
        public bool isOverHeated { get; private set; } = false;
        [SerializeField] private VehicleSkillReplacement[] _skillReplacements = Array.Empty<VehicleSkillReplacement>();
        [SerializeField] private float _maxHeat;
        [SerializeField] private float _passiveHeatDissipation;
        [SerializeField] private EntityStateMachine[] _stateMachinesToStun = Array.Empty<EntityStateMachine>();
        [NonSerialized] private Cargo _cargo;
        public CharacterBody characterBody { get; private set; }
        public SkillManager skillManager { get; private set; }
        public bool isInCombatMode
        {
            get => _isInCombatMode;
            set
            {
                if (_isInCombatMode != value)
                {
                    _isInCombatMode = value;
                    OnCombatModeChange();
                }
            }
        }
        private bool _isInCombatMode;

        private float overheatHeatDissipation => maxHeat / HeatStunState.stunDuration;

        private void Awake()
        {
            characterBody = GetComponent<CharacterBody>();
            skillManager = GetComponent<SkillManager>();
            _cargo ??= new Cargo(10);
            _maxHeat = _maxHeat == 0 ? 10 : _maxHeat;
#if DEBUG
            _cachedGUIStyle = new GUIStyle
            {
                fontSize = 30
            };
#endif
        }

        private void Start()
        {
            _isInCombatMode = false;
        }

        public void AddHeat(float heatAmount)
        {
            heat += heatAmount;
        }

        public void RemoveHeat(float heatAmount)
        {
            if (heat == 0)
                return;

            heat -= heatAmount;
            if (heat < 0)
            {
                heat = 0;
            }
        }

        public bool TryHarvest(IHarvestable harvesteable, int desiredHarvestCount)
        {
            if (_cargo.LoadResource(harvesteable.resourceType, desiredHarvestCount))
            {
                harvesteable.Harvest(desiredHarvestCount);
                return true;
            }
            return false;
        }

        private void FixedUpdate()
        {
            if (isOverHeated)
            {
                RemoveHeat(overheatHeatDissipation * Time.fixedDeltaTime);
                if (heat <= 0)
                {
                    isOverHeated = false;
                    return;
                }
                return;
            }
            RemoveHeat(_passiveHeatDissipation * Time.fixedDeltaTime);
            if (heat > maxHeat && !isOverHeated)
            {
                isOverHeated = true;
                foreach (EntityStateMachine stateMachine in _stateMachinesToStun)
                {
                    stateMachine.SetNextState(new HeatStunState());
                }
            }
        }

        private void OnCombatModeChange()
        {
            foreach (VehicleSkillReplacement replacement in _skillReplacements)
            {
                GenericSkill genericSkill = replacement.slotToReplace != SkillSlot.None ? skillManager.GetSkillBySkillSlot(replacement.slotToReplace) : skillManager.GetSkill(replacement.skillNameWhenSlotIsNone);

                genericSkill.skillDef = isInCombatMode ? replacement.combatSkillDef : replacement.harvestSkillDef;
            }
        }

        public void PreStatRecalculation(CharacterBody body) { }

        public void PostStatRecalculation(CharacterBody body) { }

        public void GetStatCoefficients(StatModifierArgs args, CharacterBody body)
        {
            if (!isInCombatMode)
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
        [Header("DEBUG")]
        public bool printHeatOnScreen = false;
        public bool printCargoContentsOnScreen = false;
        private GUIStyle _cachedGUIStyle;

        [ContextMenu("Switch Combat Mode")]
        private void SwitchToCombatMode()
        {
            isInCombatMode = !isInCombatMode;
        }
        private void OnGUI()
        {
            if(printHeatOnScreen)
            {
                GUILayout.Label("Heat: " + heat, _cachedGUIStyle);
            }

            if(printCargoContentsOnScreen)
            {
                GUILayout.Label("Total Cargo: " + _cargo.totalCargoHeld, _cachedGUIStyle);
                foreach(ResourceDef def in ResourceCatalog.resourceDefs)
                {
                    GUILayout.Label($"{def.cachedName} count: " + _cargo.GetResourceCount(def), _cachedGUIStyle);
                }
            }
        }
#endif

        [Serializable]
        public struct VehicleSkillReplacement
        {
            public SkillSlot slotToReplace;
            public string skillNameWhenSlotIsNone;

            public SkillDef combatSkillDef;
            public SkillDef harvestSkillDef;
        }
    }
}