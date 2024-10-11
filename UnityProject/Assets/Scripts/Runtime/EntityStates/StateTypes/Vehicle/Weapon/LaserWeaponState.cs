using AC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EntityStates.Vehicle.Weapon
{
    /// <summary>
    /// Estado de vehiculo el cual lanza un laser de calor, haciendo daño al enemigo en frente
    /// </summary>
    public class LaserWeaponState : BaseVehicleWeaponState, ISkillState
    {
        [Tooltip("El efecto de laser usado cuando este estado esta activo")]
        public static GameObject laserEffect;
        [Tooltip("El tiempo entre instancias de daño causado por el laser")]
        public static float timeBetweenTicks;
        [Tooltip("El radio que tiene el laser al ser disparado")]
        public static float laserRadius;
        [Tooltip("La distancia que tiene el laser")]
        public static float laserDistance;
        [Tooltip("Coeficiente de daño que el laser usa, a partir del daño base del personaje")]
        public static float damageCoefficient;

        private float _tickStopwatch;
        private HitscanAttack _hitscanAttack;
        private RaycastHit2D[] _effectHitArray = new RaycastHit2D[2];
        private GameObject _laserEffectInstance;
        private Transform _laserStartPoint;
        private Transform _laserEndPoint;
        public override void OnEnter()
        {
            base.OnEnter();

            if(laserEffect)
            {
                _laserEffectInstance = Instantiate(laserEffect);
                if(_laserEffectInstance.TryGetComponent<LineRendererPointToPoint>(out var component))
                {
                    _laserStartPoint = component.startPoint;
                    _laserEndPoint = component.endPoint;
                }
            }

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

        public override void Update()
        {
            base.Update();
            var endPoint = transform.position + transform.up * laserDistance;
            var count = Physics2D.CircleCastNonAlloc(transform.position, laserRadius, transform.up, _effectHitArray, laserDistance, LayerIndex.entityPrecise.mask);
            for(int i = 0; i < count; i++)
            {
                if(_effectHitArray[i].collider.TryGetComponent<HurtBox>(out var component) && component.healthComponent.gameObject == gameObject)
                {
                    continue;
                }

                endPoint = _effectHitArray[i].point;
            }

            _laserStartPoint.position = transform.position;
            _laserEndPoint.position = endPoint;
        }

        public override void OnExit()
        {
            base.OnExit();
            if (_laserEffectInstance)
                Destroy(_laserEffectInstance);
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }
    }
}