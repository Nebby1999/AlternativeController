using AC;
using Nebula;

namespace EntityStates
{
    public class HeatStunState : GenericStunState
    {
        public static float heatStunDuration;
        public override void OnEnter()
        {
            stunDuration = heatStunDuration;
            base.OnEnter();
            Log("Stunned from Overheat!");
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Death;
        }
    }
}