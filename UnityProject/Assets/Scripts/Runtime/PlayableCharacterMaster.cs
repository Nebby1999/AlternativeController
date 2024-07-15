using UnityEngine;
using UnityEngine.InputSystem;

namespace AC
{
    [RequireComponent(typeof(CharacterMaster))]
    public class PlayableCharacterMaster : MonoBehaviour, ICharacterInputProvider
    {
        public Vector2 movementVector { get; private set; }

        public int rotationInput { get; private set; }

        private float _rawLeftTrackInput;
        private float _rawRightTrackInput;
        private void Update()
        {
            movementVector = new Vector2(Mathf.RoundToInt(_rawLeftTrackInput), Mathf.RoundToInt(_rawRightTrackInput));
        }

        public void OnRightTrack(InputAction.CallbackContext context)
        {
            _rawRightTrackInput = context.ReadValue<float>();
        }
        public void OnLeftTrack(InputAction.CallbackContext context)
        {
            _rawLeftTrackInput = context.ReadValue<float>();
        }
    }
}