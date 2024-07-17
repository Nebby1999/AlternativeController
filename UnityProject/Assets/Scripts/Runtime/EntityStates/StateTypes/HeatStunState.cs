using AC;
using Nebula;

namespace EntityStates
{
    public class HeatStunState : EntityState
    {
        public static float stunDuration;

        public override void OnEnter()
        {
            base.OnEnter();
            Log("Stunned from Overheat!");
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if(fixedAge > stunDuration)
            {
                outer.SetNextStateToMain();
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Death;
        }
    }
}