using UnityEngine;

namespace AC
{
    //This works but its not correct, movement should feel "Tank" like.
    [RequireComponent(typeof(Rigidbody2DMotor), typeof(CharacterBody))]
    public class Rigidbody2DCharacterController : MonoBehaviour, IRigidbody2DMotorController
    {
        public float baseRotationSpeed;
        public bool rotateTowardsDirection = false;
        public int rotationInput { get; set; }
        public Vector2 movementDirection { get; set; }
        public Vector2 velocity { get; set; }
        public float rotation { get; set; }
        public Rigidbody2DMotor rigidbody2DMotor { get; private set; }
        public CharacterBody characterBody { get; private set; }
        public float movementSpeed => characterBody.movementSpeed;

        private void Awake()
        {
            rigidbody2DMotor = GetComponent<Rigidbody2DMotor>();
            characterBody = GetComponent<CharacterBody>();
        }

        private void FixedUpdate()
        {
            var rotationQuaternion = Quaternion.AngleAxis(rotation, Vector3.forward);
            var movementDirectionRotation = rotationQuaternion * movementDirection;
            velocity = movementDirectionRotation * movementSpeed;
            rotation -= rotationInput * (baseRotationSpeed * movementSpeed) * Time.fixedDeltaTime;
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