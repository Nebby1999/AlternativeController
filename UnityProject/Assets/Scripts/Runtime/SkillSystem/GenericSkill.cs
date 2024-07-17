using System;
using UnityEngine;

namespace AC
{
    public class GenericSkill : MonoBehaviour
    {
        public string genericSkillName;
        [SerializeField] private SkillDef _defaultSkill;
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
                    _skillDef.OnUnassign(this);
                }
                _skillDef = value;
                if(_skillDef)
                {
                    _skillDef.OnAssign(this);
                }
                OnSkillChanged();
            }
        }
        private SkillDef _skillDef;
        public float cooldownTimer { get; set; }
        public uint maxStock { get; private set; }
        public uint stock { get; set; }
        public EntityStateMachine cachedStateMachine { get; private set; }

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
        public bool IsReady()
        {
            if (!skillDef)
                return false;

            return skillDef.CanExecute(this);
        }

        public bool IsInSkillState()
        {
            if (!skillDef)
                return false;

            return cachedStateMachine.currentState.GetType() == (Type)skillDef.stateType;
        }
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