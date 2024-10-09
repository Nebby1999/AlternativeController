using UnityEngine;

namespace AC
{
    /// <summary>
    /// REpresenta una habilidad ejecutada por un <see cref="Vehicle"/>
    /// 
    /// <br>Mira <see cref="SkillDef"/>, <see cref="SkillManager"/> y <see cref="GenericSkill"/> para mas informacion.</br>
    /// </summary>
    [CreateAssetMenu(menuName = "AC/Skills/Vehicle SkillDef", fileName = "New VehicleSkillDef")]
    public class VehicleSkillDef : SkillDef
    {
        [Tooltip("El vehiculo necesita tener un Cargo conectado para ejecutar esta habilidad.")]
        public bool requiresConnectedCargo;

        [Tooltip("Si el valor es mayor a 0, el vehiculo necesita una cantidad minima de calor para ejecutar esta habilidad.")]
        public float minHeatRequired;

        public override BaseSkillInstanceData OnAssign(GenericSkill skillSlot)
        {
            var instanceData = new VehicleSkillInstanceData
            {
                vehicle = skillSlot.GetComponent<Vehicle>()
            };
            return instanceData;
        }

        public override bool CanExecute(GenericSkill skillSlot)
        {
            var instanceData = (VehicleSkillInstanceData)skillSlot.instanceData;
            var baseVal = base.CanExecute(skillSlot);
            bool cargoCheckPass = requiresConnectedCargo ? instanceData.vehicle.connectedCargo : true;
            bool heatCheckPass = minHeatRequired > 0 ? instanceData.vehicle.heat > minHeatRequired : true;

            return baseVal && cargoCheckPass && heatCheckPass;
        }

        public override void Execute(GenericSkill skillSlot)
        {
            base.Execute(skillSlot);
        }

        /// <summary>
        /// Representa la instancia de skill de un GenericSkill
        /// </summary>
        public class VehicleSkillInstanceData : BaseSkillInstanceData
        {
            /// <summary>
            /// El vehiculo asociado a la habilidad.
            /// </summary>
            public Vehicle vehicle;
        }
    }
}