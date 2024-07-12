using AC;
using UnityEngine;

namespace EntityStates
{
    public class BaseCharacterMain : BaseCharacterState
    {
        public bool hasInputBank { get; private set; }
        public bool hasCharacterController { get; private set; }

        protected Vector2 moveVector;
        protected int rotationInput;

        public override void OnEnter()
        {
            base.OnEnter();
            hasInputBank = inputBank;
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
            moveVector = Vector2.zero;
        }
    }
}