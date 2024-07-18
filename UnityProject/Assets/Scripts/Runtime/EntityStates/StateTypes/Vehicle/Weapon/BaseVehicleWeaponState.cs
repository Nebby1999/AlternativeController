using AC;
using EntityStates.Vehicle;
using UnityEngine;

namespace EntityStates
{
    public class BaseVehicleWeaponState : VehicleState, ISkillState
    {
        [SerializeField]
        protected float heatGainedPerSecond;

        public GenericSkill activatorSkillSlot { get; set; }

        private SkillSlot _assignedSlot;

        public override void OnEnter()
        {
            base.OnEnter();
            _assignedSlot = hasSkillManager ? skillManager.FindSkillSlot(activatorSkillSlot) : SkillSlot.None;
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
    }
}