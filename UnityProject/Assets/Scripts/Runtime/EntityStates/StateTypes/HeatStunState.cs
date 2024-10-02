using AC;
using Nebula;
using UnityEngine;

namespace EntityStates
{
    public class HeatStunState : GenericStunState
    {
        public static float heatStunDuration;
        public static GameObject steamVFX;

        private GameObject _steamInstance;
        public override void OnEnter()
        {
            stunDuration = heatStunDuration;
            base.OnEnter();
            Log("Stunned from Overheat!");
            _steamInstance = Instantiate(steamVFX, transform);
            var ps = _steamInstance.GetComponent<ParticleSystem>();
            var main = ps.main;
            main.duration = heatStunDuration;
            ps.Play();
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Death;
        }
    }
}