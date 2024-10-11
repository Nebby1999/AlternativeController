using AC;

namespace EntityStates
{
    /// <summary>
    /// Estado base de habilidades ligadas a un <see cref="GenericSkill"/>, implementa <see cref="ISkillState"/>
    /// </summary>
    public class BaseSkillState : BaseCharacterState, ISkillState
    {
        public GenericSkill activatorSkillSlot { get; set; }

        private SkillSlot _assignedSlot;

        public override void OnEnter()
        {
            base.OnEnter();
            _assignedSlot = hasSkillManager ? skillManager.FindSkillSlot(activatorSkillSlot) : SkillSlot.None;
        }

        /// <summary>
        /// Devuelve True si el boton que inicializo esta skill esta presionado.
        /// </summary>
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

    /// <summary>
    /// Interfaz de estados el cual es usado para marcar que un estado fue inicializado por un <see cref="GenericSkill"/>
    /// </summary>
    public interface ISkillState
    {
        /// <summary>
        /// El <see cref="GenericSkill"/> que inicializo este estado.
        /// </summary>
        GenericSkill activatorSkillSlot { get; set; }
    }
}