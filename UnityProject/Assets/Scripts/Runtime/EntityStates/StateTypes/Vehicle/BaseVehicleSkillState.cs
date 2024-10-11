namespace EntityStates.Vehicle
{
    /// <summary>
    /// Estado base de habilidad de un Vehiculo
    /// </summary>
    public class BaseVehicleSkillState : BaseSkillState
    {
        /// <summary>
        /// El <see cref="AC.Vehicle"/> de la maquina de estado
        /// </summary>
        public AC.Vehicle vehicle { get; private set; }

        /// <summary>
        /// Verdadero si la maquina de estado tiene un componente de tipo <see cref="AC.Vehicle"/>
        /// </summary>
        public bool hasVehicle { get; private set; }

        public override void OnEnter()
        {
            base.OnEnter();
            hasVehicle = TryGetComponent<AC.Vehicle>(out var component);
            vehicle = component;
        }
    }
}