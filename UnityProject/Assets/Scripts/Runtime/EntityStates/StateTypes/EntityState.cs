using AC;

namespace EntityStates
{
    public class EntityState : State
    {
        protected new EntityStateMachine outer;
        public CharacterBody characterBody => outer.componentLocator.characterBody;
        public HealthComponent healthComponent => outer.componentLocator.healthComponent;
        public InputBank inputBank => outer.componentLocator.inputBank;
        public Rigidbody2DCharacterController characterController => outer.componentLocator.rigidbody2DCharacterController;
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

        public virtual InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Any;
        }
    }
}