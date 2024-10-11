using UnityEngine;

namespace AC
{
    /// <summary>
    /// Clase que implementa <see cref="IMovementStrategy"/>, utilizado para un movimiento completamente generico de un personaje.
    /// </summary>
    public class GenericMovementStrategy : IMovementStrategy
    {

        public void Initialize(object sender) { }
        public MovementStrategyOutput PerformStrategy(Transform transform, Vector2 rawMovementInput, int rawRotationInput, float movementSpeed)
        {
            //TODO: Dependiendo del input, deberiamos rotar en la direccion de movimiento.
            return new MovementStrategyOutput
            {
                movement = rawMovementInput,
                rotation = rawRotationInput
            };
        }
    }
}