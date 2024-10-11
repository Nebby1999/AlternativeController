using AC;
using UnityEngine;

namespace EntityStates
{
    /// <summary>
    /// Estado base que controla el "cuerpo" de un personaje, decide que habilidades deben ejecutarse y decide como moverse en el mundo a partir de los inputs del personaje.
    /// </summary>
    public class GenericCharacterMain : BaseCharacterMain
    {
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            GatherInputs();
            HandleMovement();
            ProcessInputs();
        }

        protected virtual void HandleMovement()
        {
            if(hasCharacterController)
            {
                //Usemos los inputs directos nomas del personaje.
                characterController.movementDirection = moveVector;
                characterController.rotationInput = rotationInput;
            }
        }

        public override void OnExit()
        {
            base.OnExit();
            if(hasCharacterController)
            {
                characterController.movementDirection = Vector2.zero;
                characterController.rotationInput = 0;
            }
        }
    }
}