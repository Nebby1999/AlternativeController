namespace EntityStates.Vehicle
{
    public class BaseVehicleSkillState : BaseSkillState
    {
        public AC.Vehicle vehicle { get; private set; }
        public bool hasVehicle { get; private set; }

        public override void OnEnter()
        {
            base.OnEnter();
            hasVehicle = TryGetComponent<AC.Vehicle>(out var component);
            vehicle = component;
        }
    }
}