using Nebula;
using Nebula.Serialization;
using System;
using Unity.Collections;
using UnityEngine;

namespace AC
{
    [RequireComponent(typeof(Rigidbody2DMotor), typeof(CharacterBody))]
    public class Rigidbody2DCharacterController : MonoBehaviour, IRigidbody2DMotorController
    {
        [SerializeField, SerializableSystemType.RequiredBaseType(typeof(IMovementStrategy))]
        private SerializableSystemType _serializedMovementStrategy = new SerializableSystemType(typeof(GenericMovementStrategy));

        public int rotationInput { get; set; }
        public Vector2 movementDirection { get; set; }
        public Vector2 velocity { get; set; }
        public float rotation { get; set; }
        public Rigidbody2DMotor rigidbody2DMotor { get; private set; }
        public CharacterBody characterBody { get; private set; }
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