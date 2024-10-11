using UnityEngine;

namespace Nebula
{
    /// <summary>
    /// Clase que contiene utilidades de matematicas
    /// </summary>
    public static class NebulaMath
    {

        /// <summary>
        /// retorna el valor promedio entre los componentes de <paramref name="vector"/>
        /// </summary>
        public static float GetAverage(Vector3 vector)
        {
            return (vector.x + vector.y + vector.z) / 3;
        }

        /// <summary>
        /// Retorna el valor promedio entre los componentes de <paramref name="vector"/>
        /// </summary>
        public static float GetAverage(Vector2 vector)
        {
            return (vector.x + vector.y) / 2;
        }

        /// <summary>
        /// Retorna un Vector3 con los valores absolutos de <paramref name="a"/>
        /// <returns></returns>
        public static Vector3 Absolute(Vector3 a)
        {
            return new Vector3(Mathf.Abs(a.x), Mathf.Abs(a.y), Mathf.Abs(a.z));
        }

        /// <summary>
        /// Retorna un Vector2 con los valores absolutos de <paramref name="a"/>
        /// </summary>
        public static Vector2 Absolute(Vector2 a)
        {
            return new Vector2(Mathf.Abs(a.x), Mathf.Abs(a.y));
        }

        /// <summary>
        /// Multiplica entre los componentes x,y,z de <paramref name="a"/> y <paramref name="b"/>
        /// </summary>
        public static Vector3 MultiplyElementWise(Vector3 a, Vector3 b)
        {
            return new Vector3(a.x * b.x, a.y * b.y, a.z * b.z);
        }

        /// <summary>
        /// Multiplica entre los componentes x,y de <paramref name="a"/> y <paramref name="b"/>
        /// </summary>
        public static Vector2 MultiplyElementWise(Vector2 a, Vector2 b)
        {
            return new Vector2(a.x * b.x, a.y * b.y);
        }

        /// <summary>
        /// Divide los componentes x,y,z de <paramref name="a"/> por <paramref name="b"/>
        /// </summary>
        public static Vector3 DivideElementWise(Vector3 a, Vector3 b)
        {
            return new Vector3(a.x / b.x, a.y / b.y, a.z / b.z);
        }

        /// <summary>
        /// Divide los componentes x,y de <paramref name="a"/> por <paramref name="b"/>
        /// </summary>
        public static Vector2 DivideElementWise(Vector2 a, Vector2 b)
        {
            return new Vector2(a.x / b.x, a.y / b.y);
        }

        /// <summary>
        /// Redondea el <paramref name="vector"/> al menor valor integer cercano.
        /// </summary>
        public static Vector3 Floor(Vector3 vector)
        {
            return new Vector3(Mathf.Floor(vector.x), Mathf.Floor(vector.y), Mathf.Floor(vector.z));
        }

        /// <summary>
        /// Redondea el <paramref name="vector"/> al menor valor integer cercano.
        /// </summary>
        public static Vector2 Floor(Vector2 vector)
        {
            return new Vector2(Mathf.Floor(vector.x), Mathf.Floor(vector.y));
        }

        /// <summary>
        /// Redondea el <paramref name="vector"/> al mayor valor integer cercano.
        /// </summary>
        public static Vector3 Ceil(Vector3 vector)
        {
            return new Vector3(Mathf.Ceil(vector.x), Mathf.Ceil(vector.y), Mathf.Ceil(vector.z));
        }

        /// <summary>
        /// Redondea el <paramref name="vector"/> al mayor valor integer cercano.
        /// </summary>
        public static Vector2 Ceil(Vector2 vector)
        {
            return new Vector2(Mathf.Ceil(vector.x), Mathf.Ceil(vector.y));
        }

        /// <summary>
        /// Redondea el <paramref name="vector"/> al integer mas cercano
        /// </summary>
        public static Vector3 Round(Vector3 vector)
        {
            return new Vector3(Mathf.Round(vector.x), Mathf.Round(vector.y), Mathf.Round(vector.z));
        }

        /// <summary>
        /// Redondea el <paramref name="vector"/> al integer mas cercano
        /// </summary>
        public static Vector2 Round(Vector2 vector)
        {
            return new Vector2(Mathf.Round(vector.x), Mathf.Round(vector.y));
        }

        /// <summary>
        /// Convierte el valor <paramref name="value"/>, que tiene los valores entre <paramref name="inMin"/> e <paramref name="inMax"/>, a un nuevo valor entre <paramref name="outMin"/> y <paramref name="outMax"/>.
        /// <br></br>
        /// Se puede usar por ejemplo, para conseguir un valor entre 0 y 1 dependiendo de valores minimos y maximos.
        /// </summary>
        /// <param name="value">El valor a remappear</param>
        /// <param name="inMin">El valor minimo posible de <paramref name="value"/></param>
        /// <param name="inMax">El valor maximo posible de <paramref name="inMax"/></param>
        /// <param name="outMin">El valor minimo del valor remappeado</param>
        /// <param name="outMax">El valor maximo del valor remappeado</param>
        /// <returns>El valor remappeado</returns>
        public static float Remap(float value, float inMin, float inMax, float outMin, float outMax)
        {
            return outMin + (value - inMin) / (inMax - inMin) * (outMax - outMin);
        }
    }
}