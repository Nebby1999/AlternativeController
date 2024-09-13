using EntityStates;
using Nebula;
using Nebula.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AC
{
    [CreateAssetMenu(fileName = "New GenericSkillDef", menuName = "AC/Skills/Generic SkillDef")]
    public class SkillDef : NebulaScriptableObject
    {
        public float baseCooldown;
        public uint requiredStock;
        public string entityStateMachineName;
        public bool beginCooldownOnStateEnd;
        public bool requireKeyPress;
        [SerializableSystemType.RequiredBaseType(typeof(EntityStates.EntityState))]
        public SerializableSystemType stateType;
        public InterruptPriority interruptStrength = InterruptPriority.Any;
        
        public virtual BaseSkillInstanceData OnAssign(GenericSkill skillSlot)
        {
            return null;
        }

        public virtual void OnUnassign(GenericSkill skillSlot)
        {
        }

        public virtual bool CanExecute(GenericSkill skillSlot)
        {
            if (skillSlot.stock >= requiredStock && skillSlot.cooldownTimer <= 0)
            {
                bool canInterruptState = skillSlot.cachedStateMachine.CanInterruptState(interruptStrength);
                return canInterruptState;
            }
            return false;
        }

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

        public virtual void OnFixedUpdate(GenericSkill skillSlot)
        {
            if (beginCooldownOnStateEnd && skillSlot.IsInSkillState())
                return;

            skillSlot.TickRecharge(Time.fixedDeltaTime);
        }

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