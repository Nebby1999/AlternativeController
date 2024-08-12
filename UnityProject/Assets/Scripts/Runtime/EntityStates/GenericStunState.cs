using UnityEngine;

namespace EntityStates
{
    public class GenericStunState : EntityState
    {
        public float stunDuration;

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if(fixedAge > stunDuration)
            {
                outer.SetNextStateToMain();
            }
        }

        public GenericStunState() 
        {
            stunDuration = 1f;
        }

        public GenericStunState(float stunDuration)
        {
            this.stunDuration = stunDuration;
        }
    }
}