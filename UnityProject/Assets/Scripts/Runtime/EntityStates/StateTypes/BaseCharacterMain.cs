using AC;
using UnityEngine;

namespace EntityStates
{
    public class BaseCharacterMain : BaseCharacterState
    {
        public bool hasCharacterController { get; private set; }

        protected Vector2 moveVector;
        protected int rotationInput;

        public override void OnEnter()
        {
            base.OnEnter();
            hasCharacterController = characterController;
        }

        protected virtual void GatherInputs()
        {
            if(hasInputBank)
            {
                moveVector = inputBank.movementInput;
                rotationInput = inputBank.rotationInput;
            }
        }

        protected virtual void ProcessInputs()
        {
            if(hasSkillManager)
            {
                HandleSkill(skillManager.primary, ref inputBank.primaryButton);
                HandleSkill(skillManager.secondary, ref inputBank.secondaryButton);
                HandleSkill(skillManager.special, ref inputBank.specialButton);
            }
            moveVector = Vector2.zero;
        }

        private void HandleSkill(GenericSkill skill, ref InputBank.Button button)
        {
            if(button.down && skill && (!skill.mustKeyPress || !button.hasPressBeenClaimed) && skill.ExecuteSkillIfReady())
            {
                button.hasPressBeenClaimed = true;
            }
        }
    }
}