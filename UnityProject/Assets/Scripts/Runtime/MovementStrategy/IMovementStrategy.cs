using UnityEngine;

namespace AC
{
    public interface IMovementStrategy
    {
        public MovementStrategyOutput PerformStrategy(Transform transform, Vector2 rawMovementInput, int rawRotationInput);
    }

    public struct MovementStrategyOutput
    {
        public Vector2 movement;
        public float rotation;
    }
}