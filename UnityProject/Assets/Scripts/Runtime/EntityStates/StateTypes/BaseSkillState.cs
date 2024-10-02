using AC;

namespace EntityStates
{
    public class BaseSkillState : BaseCharacterState, ISkillState
    {
        public GenericSkill activatorSkillSlot { get; set; }

        private SkillSlot _assignedSlot;

        public override void OnEnter()
        {
            base.OnEnter();
            _assignedSlot = hasSkillManager ? skillManager.FindSkillSlot(activatorSkillSlot) : SkillSlot.None;
        }

        public virtual bool IsSkillDown()
        {
            if (!hasInputBank)
                return false;

            switch(_assignedSlot)
            {
                case SkillSlot.Primary:
                    return inputBank.primaryButton.down;
                case SkillSlot.Secondary:
                    return inputBank.secondaryButton.down;
                case SkillSlot.Special:
                    return inputBank.specialButton.down;
                default:
                    return false;
            }
        }
    }

    public interface ISkillState
    {
        GenericSkill activatorSkillSlot { get; set; }
    }
}