using AC;

namespace EntityStates.Bug
{
    public class Bite : BaseCharacterState
    {
        public static float biteDuration;
        public static float damageCoefficient;

        private float _duration;
        public override void OnEnter()
        {
            base.OnEnter();
            _duration = biteDuration / attackSpeedStat;

            Log("Chomp!");
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (fixedAge > _duration)
                outer.SetNextStateToMain();
        }

        public override void OnExit()
        {
            base.OnExit();
            Log("Chomp!");
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}