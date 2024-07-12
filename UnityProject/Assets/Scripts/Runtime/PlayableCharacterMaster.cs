using UnityEngine;
using UnityEngine.InputSystem;

namespace AC
{
    [RequireComponent(typeof(CharacterMaster))]
    public class PlayableCharacterMaster : MonoBehaviour, ICharacterInputProvider
    {
        public Vector2 movementVector { get; private set; }

        public int rotationInput { get; private set; }

        private float _rawAccelerateInput;
        private float _rawRotationInput;
        private void Update()
        {
            movementVector = new Vector2(0, Mathf.RoundToInt(_rawAccelerateInput));
            rotationInput = Mathf.RoundToInt(_rawRotationInput);
        }

        public void OnRotate(InputAction.CallbackContext context)
        {
            _rawRotationInput = context.ReadValue<float>();
        }
        public void OnAccelerate(InputAction.CallbackContext context)
        {
            _rawAccelerateInput = context.ReadValue<float>();
        }
    }
}