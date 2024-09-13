using UnityEngine;

namespace AC
{
    [CreateAssetMenu(menuName = "AC/Skills/Vehicle SkillDef", fileName = "New VehicleSkillDef")]
    public class VehicleSkillDef : SkillDef
    {
        public bool requiresConnectedCargo;
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

        public class VehicleSkillInstanceData : BaseSkillInstanceData
        {
            public Vehicle vehicle;
        }
    }
}