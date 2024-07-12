using AC;

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
    }
}