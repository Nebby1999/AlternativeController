using UnityEngine;

namespace AC
{
    public class AcceleratingTankMovementStrategy : IMovementStrategy
    {
        public Vehicle vehicle { get; private set; }
        public Transform leftPivot { get; private set; }
        public Transform rightPivot { get; private set; }
        public float maxSpeed => vehicle.characterBody.movementSpeed;
        public float accelerationStrength => vehicle.accelerationStrength;

        private float _rightTrack;
        private float _leftTrack;
        private float _speed; //This is the speed at which we change the position of the character.

        public void Initialize(object sender)
        {
            if(sender is Rigidbody2DCharacterController component && component.TryGetComponent<Vehicle>(out var v))
            {
                vehicle = v;
                leftPivot = vehicle.leftTrackTransformPoint;
                rightPivot = vehicle.rightTrackTransformPoint;
            }
        }

        public MovementStrategyOutput PerformStrategy(Transform transform, Vector2 rawMovementInput, int rawRotationInput)
        {
            MovementStrategyOutput output = default;
            _rightTrack = rawMovementInput.x;
            _leftTrack = rawMovementInput.y;
            DoSpeedAcceleration(); //Update the speed to use for this frame

            //The pivot from which we'll rotate around if needed.
            Vector3 pivot = GetPivot();

            Vector3 steerDirection = pivot - transform.position;
            steerDirection.Normalize();
            var upVector = transform.up;
            var angle = Vector2.SignedAngle(steerDirection, upVector);

            output.movementUnitsPerSecond = Vector2.up * _speed;
            output.rotationDegreesPerSecond = angle * _speed;
            return output;
        }

        private void DoSpeedAcceleration()
        {
            var magnitude = (_rightTrack + _leftTrack) / 2; //Use absolute value
            var speedAddition = (accelerationStrength * magnitude);
            var deltaTime = Time.fixedDeltaTime;
            float newSpeed;
            if (speedAddition == 0) //No inputs, we should decrease speed until 0 is reached.
            {
                if (_speed == 0) //speed is already 0, we good.
                {
                    //Debug.Log("No speed change");
                    return;
                }
                float currentSpeedSign = Mathf.Sign(_speed);
                speedAddition = currentSpeedSign == -1 ? accelerationStrength : -accelerationStrength; //Check if we should increase or decrease the speed, this allows us to see from what direction we're approaching 0
                newSpeed = _speed + (speedAddition * deltaTime);

                if(currentSpeedSign == 1) //We're currently approaching 0 from the positive side
                {
                    //Debug.Log("Approaching 0 from positive");
                    _speed = Mathf.Max(newSpeed, 0);
                }
                else
                {
                    //Debug.Log("Approaching 0 from negative");
                    _speed = Mathf.Min(newSpeed, 0);
                }
                Debug.Log(_speed);
                return;
            }
            //Increase the speed, regardless if its negative or possitive.
            newSpeed = _speed + (speedAddition * deltaTime);
            if(Mathf.Sign(newSpeed) == -1)
            {
                _speed = Mathf.Max(newSpeed, -maxSpeed);
            }
            else
            {
                _speed = Mathf.Min(newSpeed, maxSpeed);
            }
            Debug.Log(_speed);
        }


        private Vector3 GetPivot()
        {
            var t = 0.5f;
            t -= Mathf.Abs(_leftTrack) / 2;
            t += Mathf.Abs(_rightTrack) / 2;

            return Vector3.Lerp(leftPivot.position, rightPivot.position, t);
        }
    }
}