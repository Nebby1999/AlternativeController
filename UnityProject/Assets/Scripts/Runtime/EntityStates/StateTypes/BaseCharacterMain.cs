using AC;
using UnityEngine;

namespace EntityStates
{
    /// <summary>
    /// Estado base para manejar el movimiento y acciones de un <see cref="CharacterBody"/>
    /// </summary>
    public class BaseCharacterMain : BaseCharacterState
    {
        /// <summary>
        /// Verdadero si la maquina tiene un <see cref="AC.Rigidbody2DCharacterController"/>
        /// </summary>
        public bool hasCharacterController { get; private set; }

        /// <summary>
        /// El input de movimiento sacado del InputBank del personaje.
        /// </summary>
        protected Vector2 moveVector;

        /// <summary>
        /// Input de rotacion sacado del InputBank del personaje
        /// </summary>
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