using AC;
using UnityEngine;

namespace EntityStates
{
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