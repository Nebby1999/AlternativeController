using UnityEngine;

namespace AC
{
    public static class Util
    {
        public static GUIStyle debugGUIStyle = GUIStyle.none;

        public static bool CharacterRaycast(GameObject character, Ray ray, float maxDistance, LayerMask layerMask, out RaycastHit2D hit, float minDepth = Mathf.NegativeInfinity, float maxDepth = Mathf.Infinity)
        {
            var hits = Physics2D.RaycastAll(ray.origin, ray.direction, maxDistance, layerMask, minDepth, maxDepth);
            bool result = HandleCharacterPhysicsCastResults(character, ray, hits.Length, hits, out hit);
            return result;
        }

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