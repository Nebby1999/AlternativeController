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

        [SerializeField]
        private bool _doMovementInputSmoothing;

        [SerializeField]
        private float _movementInputSmoothingTime;

        private float _leftTrackInputSpeed;
        private float _currentLeftTrackInput;
        private float _rawLeftTrackInput;

        private float _rightTrackInputSpeed;
        private float _currentRightTrackInput;
        private float _rawRightTrackInput;

        private bool _rawPrimaryInput;
        private bool _rawSecondaryInput;
        private void Update()
        {
            _currentLeftTrackInput = SmoothInput(_currentLeftTrackInput, _rawLeftTrackInput, ref _leftTrackInputSpeed, _movementInputSmoothingTime);
            _currentRightTrackInput = SmoothInput(_currentRightTrackInput, _rawRightTrackInput, ref _rightTrackInputSpeed, _movementInputSmoothingTime);
            movementVector = new Vector2(_currentLeftTrackInput, _currentRightTrackInput);
        }

        private float SmoothInput(float current, float target, ref float currentVelocity, float smoothTime)
        {
            if(!_doMovementInputSmoothing)
            {
                return target;
            }

            var result = Mathf.SmoothDamp(current, target, ref currentVelocity, smoothTime);

            if(Mathf.Abs(result - target) < 0.0001f)
            {
                result = target;
            }
            return result;
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