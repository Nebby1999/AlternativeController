using AC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EntityStates.Vehicle.Weapon
{
    public class LaserWeaponState : BaseVehicleWeaponState, ISkillState
    {
        public static float timeBetweenTicks;
        public static float laserRadius;
        public static float laserDistance;
        public static float damageCoefficient;

        private float _tickStopwatch;
        private HitscanAttack _hitscanAttack;
        public override void OnEnter()
        {
            base.OnEnter();
            _hitscanAttack = new HitscanAttack
            {
                attacker = gameObject,
                hitscanCount = 1,
                hitscanRadius = laserRadius,
                distance = laserDistance,
                minAngleSpread = 0,
                maxAngleSpread = 0,
                damage = damageStat * damageCoefficient,
                falloffCalculation = HitscanAttack.BuckshotFalloffCalculation
            };
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (!vehicle.isOverHeated)
                vehicle.AddHeat(heatGainedPerSecond * Time.fixedDeltaTime);

            _tickStopwatch += Time.fixedDeltaTime;
            if(_tickStopwatch > timeBetweenTicks)
            {
                _tickStopwatch -= timeBetweenTicks;
                _hitscanAttack.origin = transform.position;
                _hitscanAttack.direction = transform.up;
                _hitscanAttack.Fire();
            }
            if (!IsSkillDown())
                outer.SetNextStateToMain();
        }
    }
}