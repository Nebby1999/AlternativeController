using AC;
using UnityEngine;

namespace EntityStates.Vehicle
{
    /// <summary>
    /// Estado de Vehiculo el cual suelta un Decoy de su Cargo actual.
    /// </summary>
    public class DeployDecoy : BaseVehicleSkillState
    {
        [Tooltip("Tiempo base entre soltar decoys")]
        public static float timeBetweenDeploys;

        private static float _stopwatch;
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            _stopwatch += Time.fixedDeltaTime;
            if(_stopwatch > timeBetweenDeploys)
            {
                _stopwatch -= timeBetweenDeploys;
                TryDeploy();
            }

            if(!IsSkillDown())
            {
                outer.SetNextStateToMain();
            }
        }

        private void TryDeploy()
        {
            if (!hasVehicle)
                return;

            var cargo = vehicle.connectedCargo;

            if(cargo)
            {
                cargo.DropResource(1);
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}