using EntityStates;
using Nebula;
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
        public Transform leftTrackTransformPoint;
        public Transform rightTrackTransformPoint;
        public float accelerationTime => _accelerationTime;
        [SerializeField] private float _accelerationTime;
        [SerializeField] private VehicleSkillReplacement[] _skillReplacements = Array.Empty<VehicleSkillReplacement>();
        [SerializeField] private float _maxHeat;
        [SerializeField] private float _passiveHeatDissipation;
        [SerializeField] private EntityStateMachine[] _stateMachinesToStun = Array.Empty<EntityStateMachine>();

        [Header("TEMP: Heat Visuals")]
        //TODO: Re-emplazar esto por algo que ocupe el mesh del vehiculo. dado que esto solo funciona con 2d
        [SerializeField] private SpriteRenderer _heatRenderer;
        [SerializeField] private Gradient _heatGradient;
        [SerializeField] private float _extremeColorThreshold = 0.9f;
        [SerializeField] private Color _extremeHeatColor;
        public Cargo connectedCargo
        {
            get => _connectedCargo;
            set
            {
                if(!_connectedCargo)
                {
                    _connectedCargo = value;
                    characterBody.SetStatsDirty();
                    return;
                }

                if (value && value.connectedVehicle != this)
                    return;

                _connectedCargo = value;
                characterBody.SetStatsDirty();
            }
        }
        private Cargo _connectedCargo;
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

        private float overheatHeatDissipation => maxHeat / HeatStunState.heatStunDuration;

        private void Awake()
        {
            characterBody = GetComponent<CharacterBody>();
            skillManager = GetComponent<SkillManager>();
            _maxHeat = _maxHeat == 0 ? 10 : _maxHeat;
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
            if(connectedCargo && connectedCargo.LoadResource(harvesteable.resourceType, desiredHarvestCount))
            {
                harvesteable.Harvest(desiredHarvestCount);
                return true;
            }
            return false;
        }

        private void Update()
        {
            var heatRemap = NebulaMath.Remap(heat, 0, maxHeat, 0, 1);
            if (heatRemap > _extremeColorThreshold)
            {
                _heatRenderer.color = Time.frameCount % 2 == 1 ? _heatGradient.Evaluate(heatRemap) : _extremeHeatColor;
                return;
            }
            _heatRenderer.color = _heatGradient.Evaluate(heatRemap);
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

            if(connectedCargo)
            {
                connectedCargo.DetachCargo();
            }

            characterBody.SetStatsDirty();
        }

        public void PreStatRecalculation(CharacterBody body) { }

        public void PostStatRecalculation(CharacterBody body) { }

        public void GetStatCoefficients(StatModifierArgs args, CharacterBody body)
        {
            if (!isInCombatMode)
            {
                args.movementSpeedMultAdd -= 0.2f;
                if(connectedCargo)
                {
                    args.movementSpeedMultAdd -= 0.2f;
                }
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