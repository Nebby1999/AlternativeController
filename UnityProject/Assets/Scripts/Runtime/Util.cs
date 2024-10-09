using UnityEngine;

namespace AC
{
    /// <summary>
    /// Clase que contiene metodos de utilidad para el proyecto.
    /// </summary>
    public static class Util
    {
        /// <summary>
        /// Una extension de <see cref="Physics2D"/> RaycastAll, el cual es usado para ejectuar un raycast desde la prespectiva de <paramref name="character"/>, el cual evita colisionar consigo mismo.
        /// </summary>
        /// <param name="character">El personaje que esta haciendo el Raycast.</param>
        /// <param name="ray">El rayo en si</param>
        /// <param name="maxDistance">La distancia maxima del rayo</param>
        /// <param name="layerMask">Cuales layers debemos colisionar</param>
        /// <param name="hit">La informacion de raycast</param>
        /// <param name="minDepth">La minima profundidad en Z para el Raycast.</param>
        /// <param name="maxDepth">La maxima profundidad en Z para el Raycast.</param>
        /// <returns>True si colisionamos con algo, si no, retorna false.</returns>
        public static bool CharacterRaycast(GameObject character, Ray ray, float maxDistance, LayerMask layerMask, out RaycastHit2D hit, float minDepth = Mathf.NegativeInfinity, float maxDepth = Mathf.Infinity)
        {
            var hits = Physics2D.RaycastAll(ray.origin, ray.direction, maxDistance, layerMask, minDepth, maxDepth);
            bool result = HandleCharacterPhysicsCastResults(character, ray, hits.Length, hits, out hit);
            return result;
        }

        /// <summary>
        /// Una extension de <see cref="Physics2D"/> CircleCastAll, el cual es usado para ejectuar un Circlecast desde la prespectiva de <paramref name="character"/>, el cual evita colisionar consigo mismo.
        /// </summary>
        /// <param name="character">El personaje que esta haciendo el Raycast.</param>
        /// <param name="ray">El origen y direccion del Circlecast</param>
        /// <param name="radius">El radio del Circulo</param>
        /// <param name="maxDistance">La distancia maxima del rayo</param>
        /// <param name="layerMask">Cuales layers debemos colisionar</param>
        /// <param name="hit">La informacion de raycast</param>
        /// <param name="minDepth">La minima profundidad en Z para el Raycast.</param>
        /// <param name="maxDepth">La maxima profundidad en Z para el Raycast.</param>
        /// <returns>True si colisionamos con algo, si no, retorna false.</returns>
        public static bool CharacterCirclecast(GameObject character, Ray ray, float radius, float maxDistance, LayerMask layerMask, out RaycastHit2D hit, float minDepth = Mathf.NegativeInfinity, float maxDepth = Mathf.Infinity)
        {
            var hits = Physics2D.CircleCastAll(ray.origin, radius, ray.direction, maxDistance, layerMask, minDepth, maxDepth);
            bool result = HandleCharacterPhysicsCastResults(character, ray, hits.Length, hits, out hit);
            return result;
        }

        private static bool HandleCharacterPhysicsCastResults(GameObject character, Ray ray, int totalHits, RaycastHit2D[] hits, out RaycastHit2D hit)
        {
            int closestIndex = -1;
            float closestDistance = float.PositiveInfinity;

            for(int i = 0; i < totalHits; i++)
            {
                var currentHit = hits[i];
                if(character == currentHit.collider.gameObject)
                {
                    continue; //We're not looking to cast to ourselves.
                }
                float dist = currentHit.distance;
                if(!(dist < closestDistance))
                {
                    continue; //The hit is farther away
                }
                if(currentHit.collider.TryGetComponent<HurtBox>(out var hb))
                {
                    HealthComponent hc = hb.healthComponent;
                    if(hc && hc.gameObject == character)
                    {
                        continue; //We collided with one of our hurtboxes
                    }
                }
                if(dist == 0)
                {
                    hit = hits[i];
                    hit.point = ray.origin;
                    return true;
                }
                closestIndex = i;
                closestDistance = dist;
            }
            if(closestIndex == -1)
            {
                hit = default;
                return false; //we didnt collide with anything
            }
            hit = hits[closestIndex];
            return true; //We've hit something
        }
    }
}