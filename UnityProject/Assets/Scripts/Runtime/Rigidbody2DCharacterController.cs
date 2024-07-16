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

        public float baseRotationSpeed;
        public float drag;
        public int rotationInput { get; set; }
        public Vector2 movementDirection { get; set; }
        public Vector2 velocity { get; set; }
        public float rotation { get; set; }
        public Rigidbody2DMotor rigidbody2DMotor { get; private set; }
        public CharacterBody characterBody { get; private set; }
        public float movementSpeed => characterBody.movementSpeed;

        private IMovementStrategy _movementStrategy;

        private void Awake()
        {
            rigidbody2DMotor = GetComponent<Rigidbody2DMotor>();
            characterBody = GetComponent<CharacterBody>();
            _movementStrategy = (IMovementStrategy)Activator.CreateInstance((Type)_serializedMovementStrategy);
        }

        private void FixedUpdate()
        {
            var movementStrategyOutput = _movementStrategy.PerformStrategy(transform, movementDirection, rotationInput);

            var rotationQuaternion = Quaternion.AngleAxis(rotation, Vector3.forward);
            Vector3 movementVector = movementStrategyOutput.movement;
            Vector3 movementDirectionRotation = rotationQuaternion * movementVector;
            Vector3 finalMovementVector = movementDirectionRotation * movementSpeed;
            velocity = Vector3.MoveTowards(velocity, finalMovementVector, drag);

            var rotationSpeed = movementStrategyOutput.rotation * (baseRotationSpeed * movementSpeed) * Time.fixedDeltaTime;
            rotation = Mathf.MoveTowardsAngle(rotation, rotation - rotationSpeed, float.PositiveInfinity);
            //rotation -= movementStrategyOutput.rotation * (baseRotationSpeed * movementSpeed) * Time.fixedDeltaTime;
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