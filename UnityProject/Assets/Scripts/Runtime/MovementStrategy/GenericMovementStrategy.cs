using UnityEngine;

namespace AC
{
    public class GenericMovementStrategy : IMovementStrategy
    {
        public void Initialize(object sender) { }
        public MovementStrategyOutput PerformStrategy(Transform transform, Vector2 rawMovementInput, int rawRotationInput, float movementSpeed)
        {
            return new MovementStrategyOutput
            {
                movement = rawMovementInput,
                rotation = rawRotationInput,
                finalMovementSpeed = movementSpeed
            };
        }
    }
}