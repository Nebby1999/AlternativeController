using Nebula;
using UnityEngine;

namespace AC
{
    /// <summary>
    /// Clase que implementa <see cref="IMovementStrategy"/>, utilizado para darle al personaje un movimiento parecido a una maquinaria pesada o un tanque, normalmente usado por <see cref="Vehicle"/>
    /// </summary>
    public class TankMovementStrategy : IMovementStrategy
    {
        /// <summary>
        /// El Vehiculo el cual esta estrategia esta usando.
        /// </summary>
        public Vehicle vehicle;
        
        /// <summary>
        /// El pivote izquierdo el cual esta estrategia usa, utilizado para simular la Oruga izquierda.
        /// </summary>
        public Transform leftPivot;

        /// <summary>
        /// El pivote derecho el cual esta estrategia usa, utilizado para simular la Oruga derecha.
        /// </summary>
        public Transform rightPivot;
        public void Initialize(object sender)
        {
            if (sender is MonoBehaviour behaviour && behaviour.TryGetComponent<Vehicle>(out vehicle))
            {
                leftPivot = vehicle.leftTrackTransformPoint;
                rightPivot = vehicle.rightTrackTransformPoint;
            }
        }

        public MovementStrategyOutput PerformStrategy(Transform transform, Vector2 rawMovementInput, int _, float movementSpeed)
        {
            MovementStrategyOutput result = new MovementStrategyOutput();

            //El input X e Y representan la oruga izquierda y derecha respectivamente, enves de ser un movimiento horizontal y vertical.
            var leftTrack = rawMovementInput.x;
            var rightTrack = rawMovementInput.y;

            var normalizedTrack = (leftTrack + rightTrack) / 2; //Creamos un valor normalizado de los inputs del track, este valor normalizado es usado para calcular la velocidad deseada.

            var v1 = GetPivot(leftTrack, rightTrack) - transform.position; //Conjseguimos el pivote actual de movimiento, y generamos una direccion hacia esta.
            v1.Normalize();
            var v2 = transform.up;
            var angle = Vector2.SignedAngle(v1, v2); //Creamos un angulo el cual muestra hacia que direccion debemos rotar.

            var speed = movementSpeed * normalizedTrack;
            result.movement = new Vector2(0, normalizedTrack); //Sin importar las orugas, siempre nos vamos a mover exclusivamente en el eje Y, el cual luego es rotado por el Charactercontroller y da la direccion correcta del tanque.

            if (v1 == Vector3.zero) //Esto es para manejar cuando ambas orugas estan en direcciones opuestas, en esta situacion el tanque deberia rotar en su propio eje.
            {
                angle = leftTrack > 0 && rightTrack < 0 ? 90 : leftTrack < 0 && rightTrack > 0 ? -90 : 0;
                speed = movementSpeed / 2;
            }

            //Calculamos la rotacion deseada y devolvemos el resultado.
            result.rotation = angle * Time.fixedDeltaTime * speed;
            return result;
        }

        private Vector3 GetPivot(float leftTrackInput, float rightTrackInput)
        {
            var t = 0.5f; //0.5F es equivalente al centro del vehiculo.

            t -= Mathf.Abs(leftTrackInput) / 2; //Los inputs varian desde el -1 hasta el 1, solamente nos interesa el valor absoluto, lo dividimos en 2 y lo restamos para saber si usamos el pivote izquierdo.
            t += Mathf.Abs(rightTrackInput) / 2; //Aumentamos para ver si usamos el pivote derecho.

            //Dependiendo del valor de T se utilizara el pivote izquierdo, el derecho, o un valor entre medio.
            //if t == -1 return leftPivot.position
            //if t == 1 return rightPivot.position
            return Vector3.Lerp(leftPivot.position, rightPivot.position, t);
        }
    }
}