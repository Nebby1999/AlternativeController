using UnityEngine;

namespace AC
{
    public class GenericMovementStrategy : IMovementStrategy
    {
        public MovementStrategyOutput PerformStrategy(Transform transform, Vector2 rawMovementInput, int rawRotationInput)
        {
            return new MovementStrategyOutput
            {
                movement = rawMovementInput,
                rotation = rawRotationInput,
            };
        }
    }
}