using AC;

namespace EntityStates.Vehicle
{
    public class Defend : BaseVehicleSkillState
    {
        public override void OnEnter()
        {
            base.OnEnter();
            if (hasVehicle)
                vehicle.isDefending = true;

            Log("Defend Enter");
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (!IsSkillDown())
                outer.SetNextStateToMain();
        }

        public override void OnExit()
        {
            base.OnExit();
            if (hasVehicle)
                vehicle.isDefending = false;
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}