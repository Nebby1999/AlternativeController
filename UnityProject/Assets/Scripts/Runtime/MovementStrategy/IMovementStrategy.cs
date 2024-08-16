using UnityEngine;

namespace AC
{
    public interface IMovementStrategy
    {
        public void Initialize(object sender);
        public MovementStrategyOutput PerformStrategy(Transform transform, Vector2 rawMovementInput, int rawRotationInput, float movementSpeed);
    }

    public struct MovementStrategyOutput
    {
        public Vector2 movement;
        public float rotation;
    }
}