using AC;
using UnityEngine;

namespace EntityStates.Vehicle
{
    public class DeployDecoy : BaseVehicleSkillState
    {
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