using Nebula;
using UnityEngine;

namespace AC
{
    public class TankMovementStrategy : IMovementStrategy
    {
        public Vehicle vehicle;
        public Transform leftPivot;
        public Transform rightPivot;
        public void Initialize(object sender)
        {
            if (sender is MonoBehaviour behaviour && behaviour.TryGetComponent<Vehicle>(out vehicle))
            {
                leftPivot = vehicle.leftTrackTransformPoint;
                rightPivot = vehicle.rightTrackTransformPoint;
            }
        }

        public MovementStrategyOutput PerformStrategy(Transform transform, Vector2 rawMovementInput, int _, float movementSpeed)
        {
            MovementStrategyOutput result = new MovementStrategyOutput();

            var leftTrack = rawMovementInput.x;
            var rightTrack = rawMovementInput.y;

            var normalizedTrack = (leftTrack + rightTrack) / 2;
            var v1 = GetPivot(leftTrack, rightTrack) - transform.position;
            v1.Normalize();
            var v2 = transform.up;
            var angle = Vector2.SignedAngle(v1, v2);

            var speed = movementSpeed * normalizedTrack;
            result.movement = new Vector2(0, normalizedTrack);

            if (v1 == Vector3.zero)
            {
                angle = leftTrack > 0 && rightTrack < 0 ? 90 : leftTrack < 0 && rightTrack > 0 ? -90 : 0;
                speed = movementSpeed / 2;
            }
            result.rotation = angle * Time.fixedDeltaTime * speed;
            return result;
        }

        private Vector3 GetPivot(float leftTrackInput, float rightTrackInput)
        {
            var t = 0.5f;
            t -= Mathf.Abs(leftTrackInput) / 2;
            t += Mathf.Abs(rightTrackInput) / 2;

            return Vector3.Lerp(leftPivot.position, rightPivot.position, t);
        }
    }
}