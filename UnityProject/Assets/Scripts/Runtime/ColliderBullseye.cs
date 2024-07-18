using Nebula;
using UnityEngine;

namespace AC
{
    [RequireComponent(typeof(Collider2D))]
    public class ColliderBullseye : MonoBehaviour
    {
        [SerializeField] private Collider2D _collider2D;

        private void OnEnable()
        {
            InstanceTracker.Add(this);
        }

        private void OnDisable()
        {
            InstanceTracker.Remove(this);
        }
    }
}