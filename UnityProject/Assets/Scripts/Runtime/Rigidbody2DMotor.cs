using UnityEngine;

namespace AC
{
    /// <summary>
    /// Un motor de personaje Kinematico usando fisicas de <see cref="Rigidbody2D"/>
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public class Rigidbody2DMotor : MonoBehaviour
    {
        /// <summary>
        /// El <see cref="Rigidbody2D"/> asociado con este motor.
        /// </summary>
        public new Rigidbody2D rigidbody2D { get; private set; }

        /// <summary>
        /// El controlador para este motor, el cual le dice al motor como funcionar.
        /// </summary>
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

    /// <summary>
    /// Representa metodos para controlar un <see cref="Rigidbody2DMotor"/>
    /// </summary>
    public interface IRigidbody2DMotorController
    {
        /// <summary>
        /// Metodo que se ejecuta cuando el motor tiene que actualizar su velocidad actual.
        /// </summary>
        /// <param name="velocity">La velocidad del motor, sobre-escribe este valor si es necesario.</param>
        void UpdateVelocity(ref Vector2 velocity);

        /// <summary>
        /// Metodo que se ejecuta cuando el motor tiene que actualizar su rotacion actual.
        /// </summary>
        /// <param name="rotation">La rotacion actual del motor, sobreescribe este valor si es necesario.</param>
        void UpdateRotation(ref float rotation);
    }
}