using AC;

namespace EntityStates
{
    public class EntityState : State
    {
        protected new EntityStateMachine outer;

        public override void OnEnter()
        {
            base.OnEnter();
            outer = base.outer as EntityStateMachine;
        }

        protected override void Initialize()
        {
            EntityStateCatalog.InitializeStateField(this);
        }
    }
}