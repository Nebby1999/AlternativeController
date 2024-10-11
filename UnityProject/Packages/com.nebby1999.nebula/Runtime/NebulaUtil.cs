using System;
using System.Collections;
using System.Linq.Expressions;
using UnityEngine;

namespace Nebula
{
    /// <summary>
    /// Representa metodos de utilidad de unity
    /// </summary>
    public static class NebulaUtil
    {
        /// <summary>
        /// Devleuvle la camara principal que se esta usando en este momento.
        /// </summary>
        public static Camera mainCamera
        {
            get
            {
                if (!_mainCamera)
                    _mainCamera = Camera.main;
                return _mainCamera;
            }
        }
        private static Camera _mainCamera;

#if UNITY_EDITOR
        /// <summary>
        /// Propiedad exclusiva de modo editor.
        /// <br></br>
        /// Devuelve a la camara que esta en el SceneView
        /// </summary>
        public static Camera sceneCamera
        {
            get
            {
                if (!_sceneCamera)
                    _sceneCamera = UnityEditor.SceneView.currentDrawingSceneView.camera;
                return _sceneCamera;
            }
        }
        private static Camera _sceneCamera;
#endif

        /// <summary>
        /// Calcula el <see cref="Bounds"/> de un objeto usando sus colliders inmediatos
        /// </summary>
        /// <param name="obj">El objeto a calcular sus bounds</param>
        /// <param name="includeChildren">Si se deberian incluir colliders hijos en la busqueda</param>
        /// <param name="ignorePredicate">Un predicado para ver si un collider deberia ser ignorado</param>
        /// <returns>Los <see cref="Bounds"/> del objeto</returns>
        public static Bounds CalculateColliderBounds(GameObject obj, bool includeChildren, Func<Collider, bool> ignorePredicate = null)
        {
            Physics.SyncTransforms();
            var colliders = includeChildren ? obj.GetComponentsInChildren<Collider>(true) : obj.GetComponents<Collider>();

            var bounds = new Bounds(obj.transform.position, Vector3.one);
            if (colliders.Length == 0)
                return bounds;

            foreach (var collider in colliders)
            {
                var colliderBounds = collider.bounds;
                if (!ignorePredicate?.Invoke(collider) ?? true)
                    bounds.Encapsulate(colliderBounds);
            }
            return bounds;
        }


        /// <summary>
        /// Calcula el <see cref="Bounds"/> de un objeto usando sus renderers inmediatos
        /// </summary>
        /// <param name="obj">El objeto a calcular sus bounds</param>
        /// <param name="includeChildren">Si se deberian incluir renderers hijos en la busqueda</param>
        /// <param name="ignorePredicate">Un predicado para ver si un renderers deberia ser ignorado</param>
        /// <returns>Los <see cref="Bounds"/> del objeto</returns>
        public static Bounds CalculateRendererBounds(GameObject obj, bool includeChildren, Func<Renderer, bool> ignorePredicate = null)
        {
            var renderers = includeChildren ? obj.GetComponentsInChildren<Renderer>(true) : obj.GetComponents<Renderer>();

            var bounds = new Bounds(obj.transform.position, Vector3.zero);
            if (renderers.Length == 0)
                return bounds;

            foreach (var renderer in renderers)
            {
                var rendererBounds = renderer.bounds;
                if (!ignorePredicate?.Invoke(renderer) ?? true)
                    bounds.Encapsulate(rendererBounds);
            }
            return bounds;
        }

        /// <summary>
        /// Equivalente a <see cref="Quaternion.LookRotation(Vector3)"/>, pero se asegura de no usar un Vector3 de valor insignificante (sqrMagnitude <= Mathf.Epsilon)
        /// </summary>
        public static Quaternion SafeLookRotation(Vector3 forward)
        {
            Quaternion result = Quaternion.identity;
            if (forward.sqrMagnitude > Mathf.Epsilon)
            {
                result = Quaternion.LookRotation(forward);
            }
            return result;
        }

        /// <summary>
        /// Equivalente a <see cref="Quaternion.LookRotation(Vector3, Vector3)"/>, pero se asegura de no usar un Vector3 <paramref name="forward"/> de valor insignificante (sqrMagnitude <= Mathf.Epsilon)
        /// </summary>
        public static Quaternion SafeLookRotation(Vector3 forward, Vector3 upwards)
        {
            Quaternion result = Quaternion.identity;
            if (forward.sqrMagnitude > Mathf.Epsilon)
            {
                result = Quaternion.LookRotation(forward, upwards);
            }
            return result;
        }

        /// <summary>
        /// Dibuja los bounds de <see cref="Bounds"/> usando la clase Debug
        /// </summary>
        /// <param name="bounds">El Bounds a dibujar</param>
        /// <param name="color">El color de los rayos</param>
        /// <param name="duration">La duracion de los rayos</param>
        public static void DebugBounds(Bounds bounds, Color color, float duration)
        {
            Vector3 min = bounds.min;
            Vector3 max = bounds.max;
            Vector3 start = new Vector3(min.x, min.y, min.z);
            Vector3 vector = new Vector3(min.x, min.y, max.z);
            Vector3 vector2 = new Vector3(min.x, max.y, min.z);
            Vector3 end = new Vector3(min.x, max.y, max.z);
            Vector3 vector3 = new Vector3(max.x, min.y, min.z);
            Vector3 vector4 = new Vector3(max.x, min.y, max.z);
            Vector3 end2 = new Vector3(max.x, max.y, min.z);
            Vector3 start2 = new Vector3(max.x, max.y, max.z);
            Debug.DrawLine(start, vector, color, duration);
            Debug.DrawLine(start, vector3, color, duration);
            Debug.DrawLine(start, vector2, color, duration);
            Debug.DrawLine(vector2, end, color, duration);
            Debug.DrawLine(vector2, end2, color, duration);
            Debug.DrawLine(start2, end, color, duration);
            Debug.DrawLine(start2, end2, color, duration);
            Debug.DrawLine(start2, vector4, color, duration);
            Debug.DrawLine(vector4, vector3, color, duration);
            Debug.DrawLine(vector4, vector, color, duration);
            Debug.DrawLine(vector, end, color, duration);
            Debug.DrawLine(vector3, end2, color, duration);
        }
        /// <summary>
        /// Dibuja una cruz en <paramref name="position"/> de radio <paramref name="radius"/>, con el color <paramref name="color"/> y duracion <paramref name="duration"/>
        /// </summary>
        /// <param name="position">La posicion de la cruz</param>
        /// <param name="radius">El radio de la cruz</param>
        /// <param name="color">El color de la cruz</param>
        /// <param name="duration">La duracion de la cruz</param>
        public static void DebugCross(Vector3 position, float radius, Color color, float duration)
        {
            Debug.DrawLine(position - Vector3.right * radius, position + Vector3.right * radius, color, duration);
            Debug.DrawLine(position - Vector3.up * radius, position + Vector3.up * radius, color, duration);
            Debug.DrawLine(position - Vector3.forward * radius, position + Vector3.forward * radius, color, duration);
        }

        /// <summary>
        /// Retorna verdadero si <paramref name="paramHash"/> existe dento de <paramref name="animator"/>
        /// </summary>
        /// <param name="paramHash">El hash code del parametro</param>
        /// <param name="animator">El animador</param>
        /// <returns>Verdadero si <paramref name="paramHash"/> existe dentro de <paramref name="animator"/></returns>
        public static bool AnimatorParamExists(int paramHash, Animator animator)
        {
            if (!animator)
                return false;

            for (int i = 0; i < animator.parameterCount; i++)
            {
                if (animator.GetParameter(i).nameHash == paramHash)
                    return true;
            }
            return false;
        }
    }
}