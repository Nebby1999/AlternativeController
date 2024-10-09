using EntityStates;
using Nebula;
using Nebula.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AC
{
    /// <summary>
    /// Representa una habilidad ejecutada dentro de un <see cref="GenericSkill"/>.
    /// <br></br>
    /// Las skilldefs son exclusivamente contenedores de metadata y no modifican a un objeto de una manera directa, mas alla de colocar una maquina de estado a su nuevo estado. la parte volatil de las skills son manejadas por <see cref="GenericSkill"/> (El cooldown actual, cuantos stocks tienen, etc)
    /// </summary>
    [CreateAssetMenu(fileName = "New GenericSkillDef", menuName = "AC/Skills/Generic SkillDef")]
    public class SkillDef : NebulaScriptableObject
    {
        [Tooltip("Cuanto tiempo hay de recarga cuando esta skill def es ejecutada.")]
        public float baseCooldown;
        [Tooltip("Cuantos stocks se necesitan para ejecutar esta skill")]
        public uint requiredStock;
        [Tooltip("El nombre de la maquina de estado a modificar cuando esta skill es ejecutada")]
        public string entityStateMachineName;
        [Tooltip("El tipo de estado que esta asociado a esta skill.")]
        [SerializableSystemType.RequiredBaseType(typeof(EntityStates.EntityState))]
        public SerializableSystemType stateType;
        [Tooltip("El tiempo de recarga deberia empezar cuando la maquina de estado sale del estado especificado por esta skill")]
        public bool beginCooldownOnStateEnd;
        [Tooltip("El cuerpo debe accionar el boton para ejecutar la habilidad. mantener presionado el boton no ejecuta la habilidad.")]
        public bool requireKeyPress;
        [Tooltip("La fuerza de interrupcion para esta habilidad.")]
        public InterruptPriority interruptStrength = InterruptPriority.Any;
        
        /// <summary>
        /// Metodo llamado cuando este skillDef es asignado a <paramref name="skillSlot"/>
        /// </summary>
        /// <param name="skillSlot">El skill slot que va a conseguir la habilidad</param>
        /// <returns>por defecto null, deberia devolver un <see cref="BaseSkillInstanceData"/>, el cual es usado para manejar data de instancia de una skill.</returns>
        public virtual BaseSkillInstanceData OnAssign(GenericSkill skillSlot)
        {
            return null;
        }

        /// <summary>
        /// Metodo llamado cuando este skillDef es removido a <paramref name="skillSlot"/>
        /// </summary>
        /// <param name="skillSlot">El skill slot que esta perdiendo la habilidad</param>
        public virtual void OnUnassign(GenericSkill skillSlot)
        {
        }

        /// <summary>
        /// Revisa si la habilidad se puede ejecutar
        /// </summary>
        /// <param name="skillSlot">El <see cref="GenericSkill"/> que esta intentando ejecutar la habilidad</param>
        /// <returns>True si la habilidad se puede ejecutar</returns>
        public virtual bool CanExecute(GenericSkill skillSlot)
        {
            if (skillSlot.stock >= requiredStock && skillSlot.cooldownTimer <= 0)
            {
                bool canInterruptState = skillSlot.cachedStateMachine.CanInterruptState(interruptStrength);
                return canInterruptState;
            }
            return false;
        }

        /// <summary>
        /// Ejecuta la habilidad en el skill <paramref name="skillSlot"/>
        /// </summary>
        /// <param name="skillSlot">El skill slot que ejecutara la habilidad.</param>
        public virtual void Execute(GenericSkill skillSlot)
        {
            if (!skillSlot)
                return;

            var stateMachine = skillSlot.cachedStateMachine ? skillSlot.cachedStateMachine : EntityStateMachine.FindStateMachineByName<EntityStateMachine>(skillSlot.gameObject, entityStateMachineName);

            if (!stateMachine)
            {
                Debug.LogWarning($"{this} cannot be executed on {skillSlot.gameObject.name} because it doesnt have a state machine with name {entityStateMachineName}.", skillSlot.gameObject);
                return;
            }

            var state = EntityStateCatalog.InstantiateState(stateType);
            if(state is ISkillState iSkillState)
            {
                iSkillState.activatorSkillSlot = skillSlot;
            }

            stateMachine.SetNextState(state);
            skillSlot.stock--;
            skillSlot.cooldownTimer = baseCooldown;
        }

        /// <summary>
        /// Metodo llamado cuando <paramref name="skillSlot"/> ejecuta FixedUpdate.
        /// </summary>
        public virtual void OnFixedUpdate(GenericSkill skillSlot)
        {
            if (beginCooldownOnStateEnd && skillSlot.IsInSkillState())
                return;

            skillSlot.TickRecharge(Time.fixedDeltaTime);
        }

        /// <summary>
        /// Clase base que indica data de instancia de una habilidad.
        /// </summary>
        public class BaseSkillInstanceData
        {

        }
    }

    public enum InterruptPriority
    {
        Any = 0,
        Skill = 1,
        PrioritySkill = 2,
        Stun = 3,
        Death = 4
    }
}