using AC;
using EntityStates.Vehicle;
using UnityEngine;

namespace EntityStates
{
    /// <summary>
    /// Estado base de vehiculo que genera calor cuando se esta usando
    /// </summary>
    public class BaseVehicleWeaponState : BaseVehicleSkillState
    {
        [Tooltip("La cantidad de calor generado por segundo")]
        [SerializeField]
        protected float heatGainedPerSecond;
    }
}