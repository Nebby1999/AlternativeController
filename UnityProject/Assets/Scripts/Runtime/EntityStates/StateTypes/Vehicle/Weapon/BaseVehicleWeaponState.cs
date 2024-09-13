using AC;
using EntityStates.Vehicle;
using UnityEngine;

namespace EntityStates
{
    public class BaseVehicleWeaponState : BaseVehicleSkillState
    {
        [SerializeField]
        protected float heatGainedPerSecond;
    }
}