using AC;
using System.Diagnostics;
using UnityEngine;

namespace EntityStates.Vehicle.Weapon
{
    public class MiningState : BaseVehicleWeaponState
    {
        public static float secondsPerTick;
        public static int resourceMinedPerTick;
        public static float searchRadius;
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