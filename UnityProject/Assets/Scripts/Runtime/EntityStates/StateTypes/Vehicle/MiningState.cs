using AC;
using System.Diagnostics;
using UnityEngine;

namespace EntityStates.Vehicle.Weapon
{
    public class MiningState : VehicleState, ISkillState
    {
        public static float heatGainedPerSecond;

        public GenericSkill activatorSkillSlot { get; set; }

        private SkillSlot _assignedSlot;

        public override void OnEnter()
        {
            base.OnEnter();
            _assignedSlot = hasSkillManager ? skillManager.FindSkillSlot(activatorSkillSlot) : SkillSlot.None;
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if(!vehicle.isOverHeated)
                vehicle.AddHeat(heatGainedPerSecond * Time.fixedDeltaTime);

            if (!IsSkillDown())
                outer.SetNextStateToMain();
        }

        public virtual bool IsSkillDown()
        {
            if (!hasInputBank)
                return false;

            switch (_assignedSlot)
            {
                case SkillSlot.Primary:
                    return inputBank.primaryButton.down;
                case SkillSlot.None:
                    return false;
                default:
                    return false;
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }
    }
}