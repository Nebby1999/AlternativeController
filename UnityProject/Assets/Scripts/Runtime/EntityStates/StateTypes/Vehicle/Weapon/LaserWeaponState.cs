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
                origin = transform.position,
                hitscanCount = 1,
                direction = transform.up,
                hitscanRadius = laserRadius,
                distance = laserDistance,
                hitMask = LayerMask.GetMask("EntityPrecise"),
                stopperMask = LayerMask.GetMask("StopperMask"),
                minAngleSpread = 0,
                maxAngleSpread = 0,
                baseDamage = damageStat * damageCoefficient
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
                _hitscanAttack.Fire();
            }
            if (!IsSkillDown())
                outer.SetNextStateToMain();
        }
    }
}