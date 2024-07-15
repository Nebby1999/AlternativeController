using UnityEngine;

namespace AC
{
    public class TankMovementStrategy : IMovementStrategy
    {
        private int _leftTrackInput;
        private float _leftTrackMovement;

        private int _rightTrackInput;
        private float _rightTrackMovement;

        public MovementStrategyOutput PerformStrategy(Transform transform, Vector2 rawMovementInput, int _)
        {
            _leftTrackInput = (int)rawMovementInput.x;
            _rightTrackInput = (int)rawMovementInput.y;

            Vector2 finalMovement = CalculateMovementVector();
            float rotation = CalculateRotation();


            return new MovementStrategyOutput
            {
                movement = finalMovement,
                rotation = rotation
            };
        }

        private float CalculateRotation()
        {
            if (_leftTrackInput == 0 && _rightTrackInput == 0)
            {
                return 0;  // No rotation needed if both inputs are zero
            }

            if (_leftTrackInput == -_rightTrackInput)
            {
                // Both inputs are negative opposites
                return _leftTrackInput > _rightTrackInput ? 1 : (_leftTrackInput < _rightTrackInput ? -1 : 0);
            }

            if (_leftTrackInput != 0 && _rightTrackInput == 0)
            {
                return 0.5f;  // Only left input is active
            }

            if (_rightTrackInput != 0 && _leftTrackInput == 0)
            {
                return -0.5f;  // Only right input is active
            }

            return 0;  // Default case, should theoretically never be reached
        }
        private Vector2 CalculateMovementVector()
        {
            _leftTrackMovement = 0.5f * _leftTrackInput;
            _rightTrackMovement = 0.5f * _rightTrackInput;

            return new Vector2
            {
                x = 0,
                y = _leftTrackMovement + _rightTrackMovement
            };
        }
    }
}