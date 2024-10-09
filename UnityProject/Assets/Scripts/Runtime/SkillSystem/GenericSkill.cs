using System;
using UnityEngine;

namespace AC
{
    /// <summary>
    /// Un GenericSkill representa un componente el cual ejecuta <see cref="SkillDef"/>s a un cuerpo
    /// <para></para>
    /// Contiene la data de instancia de una skill def.
    /// </summary>
    public class GenericSkill : MonoBehaviour
    {
        [Tooltip("El nombre generico de este GenericSkill")]
        public string genericSkillName;
        [Tooltip("La skill por defecto de este GenericSkill")]
        [SerializeField] private SkillDef _defaultSkill;

        /// <summary>
        /// Revisa si el boton asociado a este Skill deberia ser presionado enves de mantenido
        /// </summary>
        public bool mustKeyPress => skillDef ? skillDef.requireKeyPress : false;

        /// <summary>
        /// La skill def asociada a este GenericSkill
        /// </summary>
        public SkillDef skillDef
        {
            get
            {
                return _skillDef;
            }
            set
            {
                if (_skillDef == value)
                    return;

                if(_skillDef)
                {
                    instanceData = null;
                    _skillDef.OnUnassign(this);
                }
                _skillDef = value;
                cachedStateMachine = null;
                if(_skillDef)
                {
                    instanceData = _skillDef.OnAssign(this);
                }
                OnSkillChanged();
            }
        }
        private SkillDef _skillDef;

        /// <summary>
        /// El tiempo de recarga de la skill
        /// </summary>
        public float cooldownTimer { get; set; }

        /// <summary>
        /// Cuantas activaciones maximas podemos tener de la skill
        /// </summary>
        public uint maxStock { get; private set; }

        /// <summary>
        /// Cuantas activaciones actuales podemos tener de la skill.
        /// </summary>
        public uint stock { get; set; }

        /// <summary>
        /// Un cache de la state machine asociada a la habilidad.
        /// </summary>
        public EntityStateMachine cachedStateMachine { get; private set; }

        /// <summary>
        /// Data de instancia de un skill def
        /// </summary>
        public SkillDef.BaseSkillInstanceData instanceData { get; private set; }

        private void Awake()
        {
            skillDef = _defaultSkill;
        }

        private void FixedUpdate()
        {
            if (!skillDef)
                return;

            skillDef.OnFixedUpdate(this);
        }

        /// <summary>
        /// Tickea el proceso de recarga de la habilidad
        /// </summary>
        /// <param name="fixedDeltaTime">El valor de Time.fixedDeltaTime</param>
        public void TickRecharge(float fixedDeltaTime)
        {
            if (stock > maxStock)
                return;

            cooldownTimer -= fixedDeltaTime;
            if (cooldownTimer <= 0)
            {
                stock += 1;
                cooldownTimer = 0;
                if (stock > maxStock)
                    stock = maxStock;
            }
        }

        private void OnSkillChanged()
        {
            stock = skillDef ? skillDef.requiredStock : 0;
            maxStock = stock;
            cachedStateMachine = skillDef ? EntityStateMachine.FindStateMachineByName<EntityStateMachine>(gameObject, skillDef.entityStateMachineName) : null;
        }

        private void ExecuteSkill()
        {
            if (!skillDef)
                return;

            skillDef.Execute(this);
        }

        /// <summary>
        /// Revisa si este GenericSkill se puede ejecutar
        /// </summary>
        /// <returns>True si se puede ejecutar</returns>
        public bool IsReady()
        {
            if (!skillDef)
                return false;

            return skillDef.CanExecute(this);
        }

        /// <summary>
        /// Revisa si la maquina de estado asociado a la habilidad esta en el estado especificado por la habilidad
        /// </summary>
        public bool IsInSkillState()
        {
            if (!skillDef)
                return false;

            return cachedStateMachine.currentState.GetType() == (Type)skillDef.stateType;
        }

        /// <summary>
        /// Ejecuta la habilidad si es posible.
        /// </summary>
        /// <returns>True si la habilidad fue ejecutada</returns>
        public bool ExecuteSkillIfReady()
        {
            var canExecute = IsReady();
            if (canExecute)
            {
                ExecuteSkill();
                return true;
            }
            return false;
        }
    }
}