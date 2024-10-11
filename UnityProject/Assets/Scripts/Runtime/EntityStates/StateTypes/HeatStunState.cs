using AC;
using Nebula;
using UnityEngine;

namespace EntityStates
{
    /// <summary>
    /// Estado de un <see cref="AC.Vehicle"/>, utilizado cuando se sobrecalienta.
    /// </summary>
    public class HeatStunState : GenericStunState
    {
        [Tooltip("La duracion de stun de este estado.")]
        public static float heatStunDuration;
        [Tooltip("Un efecto usado cuando este estado comienza.")]
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