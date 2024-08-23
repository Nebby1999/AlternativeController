using UnityEngine;

namespace AC
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Rigidbody2DMotor : MonoBehaviour
    {
        public new Rigidbody2D rigidbody2D { get; private set; }
        public IRigidbody2DMotorController controller { get; private set; }

        private Vector2 _rigidbodyVelocity;
        private float _rigidbodyRotation;
        private void Awake()
        {
            rigidbody2D = GetComponent<Rigidbody2D>();
            controller = GetComponent<IRigidbody2DMotorController>();
        }

        private void Start()
        {
            rigidbody2D.gravityScale = 0;
        }

        private void FixedUpdate()
        {
            _rigidbodyVelocity = rigidbody2D.velocity;
            _rigidbodyRotation = rigidbody2D.rotation;
            if(controller == null)
            {
                rigidbody2D.velocity = Vector2.zero;
                rigidbody2D.rotation = 0;
                return;
            }
            controller.UpdateVelocity(ref _rigidbodyVelocity);
            controller.UpdateRotation(ref _rigidbodyRotation);
            rigidbody2D.velocity = _rigidbodyVelocity;
            rigidbody2D.rotation = _rigidbodyRotation;
        }

        private void OnValidate()
        {
            GetComponent<Rigidbody2D>().gravityScale = 0;
        }
    }

    public interface IRigidbody2DMotorController
    {
        void UpdateVelocity(ref Vector2 velocity);
        void UpdateRotation(ref float rotation);
    }
}