using AC;

namespace EntityStates.Vehicle
{
    /// <summary>
    /// Estado de vehiculo el cual marca que un vehiculo esta defendiendo
    /// </summary>
    public class Defend : BaseVehicleSkillState
    {
        //TODO: Deberia aparecer una burbuja que da la idea de un escudo, y esta ser destruida cuando el estado acaba
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