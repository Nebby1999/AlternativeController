using EntityStates;
using Nebula;
using System;
using System.Reflection.Emit;
using UnityEngine;

namespace AC
{
    /// <summary>
    /// Componente  que se usa para representar un Vehiculo, el cual tiene un subestado de ataque y es manejado por un componente de <see cref="HeadQuarters"/>.
    /// 
    /// <br>Usualmente los Vehiculos son controlados por jugadores.</br>
    /// </summary>
    [RequireComponent(typeof(CharacterBody), typeof(SkillManager))]
    public class Vehicle : MonoBehaviour, IBodyStatModifier
    {
        //TODO: Segun pablo, el sistema de calor tambien se aplicaria a las bases, el sistema de calor deberia moverse a un "Heat Component"

        /// <summary>
        /// Cuanto calor tiene el vehiculo
        /// </summary>
        public float heat { get; private set; }
        /// <summary>
        /// La cantidad maxima de calor que puede soportar el vehiculo
        /// </summary>
        public float maxHeat => _maxHeat;
        /// <summary>
        /// Retorna "true" si el vehiculo esta sobrecalentado.
        /// </summary>
        public bool isOverHeated { get; private set; } = false;
        [Tooltip("Un punto de pivote el cual es usado para controlar el vehiculo.")]
        public Transform leftTrackTransformPoint;
        [Tooltip("Un punto de pivote el cual es usado para controlar el vehiculo.")]
        public Transform rightTrackTransformPoint;

        [Tooltip("Una lista de habilidades para reemplazar, las cuales se usan cuando el vehiculo cambia de estado de Combate a Recolectar, y vice-versa.")]
        [SerializeField] private VehicleSkillReplacement[] _skillReplacements = Array.Empty<VehicleSkillReplacement>();

        [Tooltip("Cuanto calor el vehiculo puede soportar antes que se sobrecaliente.")]
        [SerializeField] private float _maxHeat;

        [Tooltip("Esta cantidad de calor por segundo es perdida de manera pasiva.")]
        [SerializeField] private float _passiveHeatDissipation;

        [Tooltip("Cuando nos sobre-calentemos, que Maquinas de estado deberiamos estunnear.")]
        [SerializeField] private EntityStateMachine[] _stateMachinesToStun = Array.Empty<EntityStateMachine>();

        [Header("TEMP: VISUALES DE CALOR")]
        //TODO: reemplazar esto por algo que ocupe el mesh del vehiculo. dado que esto solo funciona con 2d
        [Tooltip("Los sprites que vamos a colorear con la cantidad de calor en el vehiculo")]
        [SerializeField] private SpriteRenderer[] _heatRenderers;
        
        [Tooltip("Un gradiente el cual representa el calor del vehiculo")]
        [SerializeField] private Gradient _heatGradient;

        [Tooltip("Que porcentaje de calor se considera calor \"Extremo\"")]
        [SerializeField, Range(0, 1)] private float _extremeColorThreshold = 0.9f;

        [Tooltip("El color usado cuando el vehiculo esta en calor extremo")]
        [SerializeField] private Color _extremeHeatColor;

        /// <summary>
        /// El <see cref="Cargo"/> actual que esta conectado a este vehiculo.
        /// 
        /// <br>Cuando esta valor es cambiado, los Stats del vehiculo son marcados como sucios para re-calcularlos.</br>
        /// </summary>
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

        /// <summary>
        /// El CharacterBody de este Vehiculo
        /// </summary>
        public CharacterBody characterBody { get; private set; }
        /// <summary>
        /// El SkillManager de este Vehiculo
        /// </summary>
        public SkillManager skillManager { get; private set; }

        /// <summary>
        /// Booleano que representa si el vehiculo esta en modo combate o no. Cuando el valor de este cambia, las habilidades del vehiculo son cambiadas y los stats se re-calculan
        /// 
        /// <br>Este valor usualmente es modificado directamente por los <see cref="HeadQuarters"/></br>
        /// </summary>
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

        //TODO: Talves un sistema de Buffs/Debuffs?
        /// <summary>
        /// Booleano que representa si el vehiculo esta en modo de defensa. Cuando este valor cambia, los stats del vehiculo son re-calculados.
        /// </summary>
        public bool isDefending
        {
            get => _isDefending;
            set
            {
                if(_isDefending != value)
                {
                    _isDefending = true;
                    characterBody.SetStatsDirty();
                }
            }
        }
        private bool _isDefending;
        
        //Cuando el vehiculo esta sobrecalentado, esta cantidad de calor es perdida durante la duracion del Stun.
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

        /// <summary>
        /// Agrega <paramref name="heatAmount"/> calor al vehiculo.
        /// </summary>
        /// <param name="heatAmount">La cantidad de calor a añadir</param>
        public void AddHeat(float heatAmount)
        {
            heat += heatAmount;
        }

        /// <summary>
        /// Reduce <paramref name="heatAmount"/> calor al vehiculo.
        /// </summary>
        /// <param name="heatAmount">La cantidad de calor a reducir</param>
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

        /// <summary>
        /// Intenta cosechar la cantidad <paramref name="desiredHarvestCount"/> al objeto <paramref name="harvesteable"/>.
        /// <br>Retorna false si el vehiculo no tiene un <see cref="connectedCargo"/>
        /// </summary>
        /// <param name="harvesteable">El componente harvesteable que estamos cosechando.</param>
        /// <param name="desiredHarvestCount">La cantidad a Cosechar</param>
        /// <returns></returns>
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
            Color color = default;
            if(heatRemap > _extremeColorThreshold)
            {
                //Cuando el calor se considera extremo, este codigo permite cambiar rapidamente entre el color extremo y el color de la gradiante, causando un flashing que avisa al jugador que se esta acercando al sobrecalentamiento.
                color = Time.frameCount % 2 == 1 ? _heatGradient.Evaluate(heatRemap) : _extremeHeatColor;
            }
            else
            {
                color = _heatGradient.Evaluate(heatRemap);
            }
            for(int i = 0; i < _heatRenderers.Length; i++)
            {
                _heatRenderers[i].color = color;
            }
        }

        private void FixedUpdate()
        {
            if (isOverHeated)
            {
                RemoveHeat(overheatHeatDissipation * Time.fixedDeltaTime); //Tenemos que disipar el calor para que llegue a 0 cuando el estado de stun acabe.
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
                //Conseguimos el GnericSkill del replacement.
                GenericSkill genericSkill = replacement.slotToReplace != SkillSlot.None ? skillManager.GetSkillBySkillSlot(replacement.slotToReplace) : skillManager.GetSkill(replacement.skillNameWhenSlotIsNone);

                //Seleccionamos el skill dependiendo del estado de combate.
                genericSkill.skillDef = isInCombatMode ? replacement.combatSkillDef : replacement.harvestSkillDef;
            }

            if(connectedCargo && isInCombatMode) //Si estamos en modo de combate, no podemos tener cargo.
            {
                connectedCargo.DetachCargo();
            }

            characterBody.SetStatsDirty();
        }

        public void PreStatRecalculation(CharacterBody body) { }

        public void PostStatRecalculation(CharacterBody body) { }

        public void GetStatCoefficients(StatModifierArgs args, CharacterBody body)
        {
            if (isDefending)
                args.baseArmorAdd += 50;
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

        /// <summary>
        /// Estructura que representa un reemplazo de habilidad para un Vehiculo.
        /// </summary>
        [Serializable]
        public struct VehicleSkillReplacement
        {
            [Tooltip("El tipo de habilidad que estamos reemplazando.")]
            public SkillSlot slotToReplace;

            [Tooltip("Si \"Slot to Replace\" es \"None\", deberiamos conseguir el GenericSkill con este nombre.")]
            public string skillNameWhenSlotIsNone;

            [Tooltip("La SkillDef que es usada cuando el vehiculo esta en modo combate.")]
            public SkillDef combatSkillDef;

            [Tooltip("La SkillDef que es usada cuando el vehiculo esta en modo cosecha.")]
            public SkillDef harvestSkillDef;
        }
    }
}