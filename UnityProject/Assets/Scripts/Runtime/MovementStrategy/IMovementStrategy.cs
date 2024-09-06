using UnityEngine;

namespace AC
{
    public interface IMovementStrategy
    {
        public void Initialize(object sender);
        public MovementStrategyOutput PerformStrategy(Transform transform, Vector2 rawMovementInput, int rawRotationInput);
    }

    public struct MovementStrategyOutput
    {
        public Vector2 movementUnitsPerSecond;
        public float rotationDegreesPerSecond;
    }
}