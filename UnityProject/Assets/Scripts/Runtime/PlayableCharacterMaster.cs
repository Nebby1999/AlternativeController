using UnityEngine;
using UnityEngine.InputSystem;

namespace AC
{
    [RequireComponent(typeof(CharacterMaster))]
    public class PlayableCharacterMaster : MonoBehaviour, ICharacterInputProvider
    {
        public Vector2 movementVector { get; private set; }

        public int rotationInput { get; private set; }

        public bool primaryInput => _rawPrimaryInput;

        public bool secondaryInput => _rawSecondaryInput;

        private float _rawLeftTrackInput;
        private float _rawRightTrackInput;
        private bool _rawPrimaryInput;
        private bool _rawSecondaryInput;
        private void Update()
        {
            movementVector = new Vector2(_rawLeftTrackInput, _rawRightTrackInput);
        }

        public void OnRightTrack(InputAction.CallbackContext context)
        {
            _rawRightTrackInput = context.ReadValue<float>();
        }

        public void OnLeftTrack(InputAction.CallbackContext context)
        {
            _rawLeftTrackInput = context.ReadValue<float>();
        }

        public void OnPrimary(InputAction.CallbackContext context)
        {
            _rawPrimaryInput = context.ReadValueAsButton();
        }

        public void OnSecondary(InputAction.CallbackContext context)
        {
            _rawSecondaryInput = context.ReadValueAsButton();
        }
    }
}