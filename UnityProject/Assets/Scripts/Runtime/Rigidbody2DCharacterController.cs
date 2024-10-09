using Nebula;
using Nebula.Serialization;
using System;
using Unity.Collections;
using UnityEngine;

namespace AC
{
    /// <summary>
    /// Una implementacion de <see cref="IRigidbody2DMotorController"/> el cual es usado para controlar un <see cref="Rigidbody2DMotor"/>
    /// </summary>
    [RequireComponent(typeof(Rigidbody2DMotor), typeof(CharacterBody))]
    public class Rigidbody2DCharacterController : MonoBehaviour, IRigidbody2DMotorController
    {
        [Tooltip("Que tecnica te movimiento usa este controlador para controlar el motor.")]
        [SerializeField, SerializableSystemType.RequiredBaseType(typeof(IMovementStrategy))]
        private SerializableSystemType _serializedMovementStrategy;

        /// <summary>
        /// El input actual de rotacion, donde -1 es rotar contra las manillas del reloj, y 1 es rotar con las manillas del reloj
        /// </summary>
        public int rotationInput { get; set; }

        /// <summary>
        /// La direccion de movimiento deseada
        /// </summary>
        public Vector2 movementDirection { get; set; }

        /// <summary>
        /// La velocidad actual del controlador
        /// </summary>
        public Vector2 velocity { get; set; }

        /// <summary>
        /// La rotacion actual del controlador.
        /// </summary>
        public float rotation { get; set; }

        /// <summary>
        /// El motor que este controlador esta controlando.
        /// </summary>
        public Rigidbody2DMotor rigidbody2DMotor { get; private set; }

        /// <summary>
        /// El <see cref="CharacterBody"/> asociado con este controlador.
        /// </summary>
        public CharacterBody characterBody { get; private set; }

        /// <summary>
        /// La velocidad de movimiento de este controlador.
        /// </summary>
        public float movementSpeed => characterBody.movementSpeed;

        private IMovementStrategy _movementStrategy;
        private Transform _transform;

        private void Awake()
        {
            rigidbody2DMotor = GetComponent<Rigidbody2DMotor>();
            characterBody = GetComponent<CharacterBody>();
            _movementStrategy = (IMovementStrategy)Activator.CreateInstance((Type)_serializedMovementStrategy);
            _movementStrategy.Initialize(this);
            _transform = transform;
        }

        private void FixedUpdate()
        {
            var tuple = _movementStrategy.PerformStrategy(_transform, movementDirection, rotationInput, movementSpeed);

            var vector = tuple.movement;
            rotation = Mathf.MoveTowardsAngle(rotation, rotation += tuple.rotation, movementSpeed);

            var desiredVelocity = Quaternion.AngleAxis(rotation, Vector3.forward) * (vector * movementSpeed);

            velocity = Vector3.MoveTowards(velocity, desiredVelocity, movementSpeed * 0.05f);

        }

        public void UpdateVelocity(ref Vector2 currentVelocity)
        {
            currentVelocity = velocity;
        }

        public void UpdateRotation(ref float currentRotation)
        {
            currentRotation = rotation;
        }
    }
}