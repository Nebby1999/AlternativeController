using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AC
{

    public class StunController : MonoBehaviour
    {
        [SerializeField] private float _stunDelay = 2f;
        private float _cooldown;
        private void Update()
        {
            if(_cooldown > 0) _cooldown -= Time.deltaTime;
            _cooldown = Mathf.Clamp(_cooldown, 0f, _stunDelay);
        }
        public void TryStun(bool input)
        {
            if(!input || _cooldown > 0f) return;

            Debug.Log("Trying to stun the closest enemy");
        
            Collider enemy = GetClosestEnemy();

            if(enemy == null)
            {
                Debug.Log("No enemy to stun");
                return;
            }

            //Stunear al enemigo

            _cooldown = _stunDelay;
        }
        private Collider GetClosestEnemy()
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, 2.5f);
            Collider closestCollider = null;
            float closestDistance = float.MaxValue;
            foreach (Collider collider in hitColliders)
            {
                if(collider.TryGetComponent<Enemy>(out Enemy enemy))
                {
                    float distance = Vector3.Distance(transform.position, collider.transform.position);

                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestCollider = collider;
                    }
                }
            }
            return closestCollider;
        }
    }
}
