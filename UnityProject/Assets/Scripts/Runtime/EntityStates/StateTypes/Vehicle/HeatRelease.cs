using AC;
using Nebula;
using UnityEngine;

namespace EntityStates.Vehicle
{
    /// <summary>
    /// Estado de vehiculo base que disipa el calor, causando un stun alrededor y najando el calor.
    /// </summary>
    public class HeatRelease : BaseVehicleState
    {
        [Tooltip("El efecto usado cuando se disipa el calor")]
        public static GameObject vfx;
        [Tooltip("Cuantas particulas se dberian emitir en relacion al calor")]
        public static AnimationCurve burstCountCurve;
        [Tooltip("El rango minimo del stun")]
        public static float minStunRange;
        [Tooltip("El rango maximo del stun")]
        public static float maxStunRange;
        [Tooltip("La duracion base de este estado.")]
        public static float baseDuration;

        private float _radius;
        private float _duration;
        public override void OnEnter()
        {
            base.OnEnter();
            Log("Enter");
            _duration = baseDuration;
            _radius = NebulaMath.Remap(vehicle.heat, 0, vehicle.maxHeat, minStunRange, maxStunRange);

            var instance = Instantiate(vfx, transform.position, Quaternion.identity);
            var particleSystem = instance.GetComponent<ParticleSystem>();
            var emission = particleSystem.emission;
            var burst = emission.GetBurst(0);
            burst.count = new ParticleSystem.MinMaxCurve(burstCountCurve.Evaluate(NebulaMath.Remap(vehicle.heat, 0, vehicle.maxHeat, 0, 1)));
            emission.SetBurst(0, burst);
#if UNITY_EDITOR
            GlobalGizmos.EnqueueGizmoDrawing(() =>
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(transform.position, _radius);
            });
#endif
            //TODO: esto talvez deberia ser una clase propia, parecido a un HitscanAttack, talvez llamarlo "BlastAttack"? sobre todo si decidimos hacer mas ataques que tienen un radio de efecto.
            CircleSearch search = new CircleSearch()
            {
                origin = transform.position,
                radius = _radius,
                useTriggers = false,
                candidateMask = LayerIndex.entityPrecise.mask
            };

            search.FindCandidates()
                .FilterCandidatesByDistinctHealthComponent()
                .FilterSearcher()
                .FilterCandidatesByTag(GameTags.ENEMY_TEAM)
                .GetResults(out var toStun);

            vehicle.RemoveHeat(vehicle.heat);

            foreach (var candidate in toStun)
            {
                //Stunea a la entidad
                var healthComponentToStun = candidate.colliderHurtbox.healthComponent;

                if(healthComponent)
                {
                    DamageInfo damageInfo = new DamageInfo
                    {
                        attackerBody = characterBody,
                        attackerObject = gameObject,
                        damage = 0,
                        isStunning = true
                    };
                    healthComponent.TakeDamage(damageInfo);
                }
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if(fixedAge >= _duration)
            {
                outer.SetNextStateToMain();
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Stun;
        }
    }
}