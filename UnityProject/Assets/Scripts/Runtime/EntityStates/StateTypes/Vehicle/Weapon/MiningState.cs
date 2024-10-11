using AC;
using System.Diagnostics;
using UnityEngine;

namespace EntityStates.Vehicle.Weapon
{
    /// <summary>
    /// Estado base de arma del vehiculo, mina en un radio pequeño cualquier <see cref="ResourceOreDeposit"/> encontrado
    /// </summary>
    public class MiningState : BaseVehicleWeaponState
    {
        [Tooltip("Cuantos segundos deberian pasar antes que se intente minar el deposito")]
        public static float secondsPerTick;
        [Tooltip("La cantidad de recursos minados cuando se intenta minar")]
        public static int resourceMinedPerTick;
        [Tooltip("El radio de busqueda de mineria")]
        public static float searchRadius;
        [Tooltip("En que layer buscar los recursos")]
        public static LayerMask searchMask;

        private CircleSearch _sphereSearch;
        private float _stopwatch;

        public override void OnEnter()
        {
            base.OnEnter();
            _sphereSearch = new CircleSearch();
            _sphereSearch.radius = searchRadius;
            _sphereSearch.candidateMask = searchMask;
            _sphereSearch.useTriggers = false;
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if(!vehicle.isOverHeated)
                vehicle.AddHeat(heatGainedPerSecond * Time.fixedDeltaTime);

            _stopwatch += Time.fixedDeltaTime;
            if(_stopwatch > secondsPerTick)
            {
                _stopwatch -= secondsPerTick;
                TryHarvestResource();
            }
            if (!IsSkillDown())
                outer.SetNextStateToMain();
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }

        private void TryHarvestResource()
        {
            _sphereSearch.origin = transform.position;
            _sphereSearch.FindCandidates()
                .OrderByDistance()
                .FilterBy(c => c.collider.gameObject.GetComponent<IHarvestable>() != null)
                .FirstOrDefault(out var candidate);

            if(candidate.collider && candidate.collider.TryGetComponent<IHarvestable>(out var harvesteable))
            {
                vehicle.TryHarvest(harvesteable, resourceMinedPerTick);
            }
        }
    }
}