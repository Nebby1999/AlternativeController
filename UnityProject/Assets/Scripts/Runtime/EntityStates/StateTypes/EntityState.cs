using AC;

namespace EntityStates
{
    /// <summary>
    /// Clase base de todos los estados que el juego ocupa.
    /// 
    /// revisa tambien <see cref="State"/> para mas informacion.
    /// </summary>
    public class EntityState : State
    {
        /// <summary>
        /// La Maquina de estado que esta corriendo este estado actual.
        /// </summary>
        protected new EntityStateMachine outer;

        /// <summary>
        /// El <see cref="CharacterBody"/> que tiene esta maquina.
        /// </summary>
        public CharacterBody characterBody => outer.componentLocator.characterBody;

        /// <summary>
        /// El <see cref="HealthComponent"/> que tiene esta maquina.
        /// </summary>
        public HealthComponent healthComponent => outer.componentLocator.healthComponent;

        /// <summary>
        /// El <see cref="InputBank"/> que tiene esta maquina.
        /// </summary>
        public InputBank inputBank => outer.componentLocator.inputBank;

        /// <summary>
        /// El <see cref="Rigidbody2DCharacterController"/> que tiene esta maquina.
        /// </summary>
        public Rigidbody2DCharacterController characterController => outer.componentLocator.rigidbody2DCharacterController;

        /// <summary>
        /// El <see cref="SkillManager"/> que tiene esta maquina.
        /// </summary>
        public SkillManager skillManager => outer.componentLocator.skillManager;

        public override void OnEnter()
        {
            base.OnEnter();
            outer = base.outer as EntityStateMachine;
        }

        protected override void Initialize()
        {
            EntityStateCatalog.InitializeStateField(this);
        }

        /// <summary>
        /// Metodo que retorna la fuerza requerida de interrupcion de un <see cref="InterruptPriority"/>
        /// <br></br>
        /// Si un <see cref="InterruptPriority"/> es mayor que el valor retornado por esto, entonces este estado es sobre-escrito.
        /// </summary>
        /// <returns>El <see cref="InterruptPriority"/> de este estado.</returns>
        public virtual InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Any;
        }
    }
}