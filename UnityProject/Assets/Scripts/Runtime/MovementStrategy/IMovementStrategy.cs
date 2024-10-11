using UnityEngine;

namespace AC
{
    /// <summary>
    /// Un <see cref="IMovementStrategy"/> es una interfaz utilizada para manejar como se movera un <see cref="Rigidbody2DMotor"/> el cual es manejado por un <see cref="Rigidbody2DCharacterController"/>. Transforma los inputs crudos de movimiento a un vector de movimiento, una rotacion deseada y una velocidad de movimiento deseada.
    /// </summary>
    public interface IMovementStrategy
    {
        /// <summary>
        /// Metodo para inicializar un <see cref="IMovementStrategy"/>, idealmente tu clase o struct deberia ser inicializado por este metodo.
        /// </summary>
        /// <param name="sender">El objeto que inicializo la clase.</param>
        public void Initialize(object sender);

        /// <summary>
        /// Metodo para crear un <see cref="MovementStrategyOutput"/> a partir de un <paramref name="transform"/>, inputs de movimiento, y velocidad de movimiento.
        /// </summary>
        /// <param name="transform">El transform actual el cual se desea mover.</param>
        /// <param name="rawMovementInput">El input deseado de movimiento</param>
        /// <param name="rawRotationInput">El input deseado de rotacion</param>
        /// <param name="movementSpeed">La velocidad de movimiento actual del transform</param>
        /// <returns>Un <see cref="MovementStrategyOutput"/>, el cual luego el <see cref="Rigidbody2DCharacterController"/> usara para moverse.</returns>
        public MovementStrategyOutput PerformStrategy(Transform transform, Vector2 rawMovementInput, int rawRotationInput, float movementSpeed);
    }

    /// <summary>
    /// Representa una estructura de datos el cual el <see cref="Rigidbody2DCharacterController"/> utiliza para moverse.
    /// </summary>
    public struct MovementStrategyOutput
    {
        /// <summary>
        /// El vector de movimiento que el <see cref="Rigidbody2DCharacterController"/> deberia usar
        /// </summary>
        public Vector2 movement;
        /// <summary>
        /// La rotacion en eje Z que el <see cref="Rigidbody2DCharacterController"/> deberia usar.
        /// </summary>
        public float rotation;
        /// <summary>
        /// La Velocidad de Movimiento que el <see cref="Rigidbody2DCharacterController"/> deberia usar.
        /// </summary>
        public float movementSpeed;
    }
}