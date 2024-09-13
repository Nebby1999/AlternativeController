using AC;
using Nebula;
using UnityEngine;

namespace EntityStates.Vehicle
{
    public class HeatRelease : BaseVehicleState
    {
        public static GameObject vfx;
        public static AnimationCurve burstCountCurve;
        public static float minStunRange;
        public static float maxStunRange;
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
                .FilterCandidatesByTeam(GameTags.ENEMY_TEAM)
                .GetResults(out var toStun);

            vehicle.RemoveHeat(vehicle.heat);

            foreach (var candidate in toStun)
            {
                //Stun the entity
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