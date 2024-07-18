using AC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EntityStates.Vehicle.Weapon
{
    public class LaserWeaponState : BaseVehicleWeaponState, ISkillState
    {
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (!vehicle.isOverHeated)
                vehicle.AddHeat(heatGainedPerSecond * Time.fixedDeltaTime);

            if (!IsSkillDown())
                outer.SetNextStateToMain();
        }
    }
}