using System.Collections;
using UnityEngine;

namespace AC
{

    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(Movement))]
    public class Enemy : MonoBehaviour, ILimitable
    {
        [SerializeField] private ResourceDef _type;
        [SerializeField] private TargetPriorityManager _priority;
        private SpriteRenderer _sprite;
        private Transform _transform;
        private Transform _target;
        private Movement _movement;
        [SerializeField] private bool _canMove;

        public Vector2 ApplyLimit(Vector2 newPosition, float offset)
        {
            Vector2 currentPosition = _transform.position;

            float distance = Vector2.Distance(currentPosition, _target.position);

            if (distance > offset)
            {
                Vector2 direction = (newPosition - currentPosition).normalized;
                Vector2 limitedPosition = currentPosition + direction * offset;
                return limitedPosition;
            }
            else
            {
                return newPosition;
            }
        }

        private void Awake()
        {
            _transform = transform;
            _movement = GetComponent<Movement>();
            _sprite = GetComponent<SpriteRenderer>();
        }
        private void Update()
        {
            _target = _priority.GetHighestPriorityTarget();
            _transform.up = _target.position - _transform.position;
            _movement.TryMovement(_canMove);
        }
        private void OnEnable()
        {
            _priority.Initialize(_transform);
            _target = _priority.GetHighestPriorityTarget();
            _movement.Configure(this, _target.localScale.y);
            StartCoroutine(EnableMovement());
            _sprite.color = _type.resourceColor;
        }
        private IEnumerator EnableMovement()
        {
            while (true)
            {
                yield return new WaitForSeconds(0.25f);
                _canMove = true;
                yield return null;
                _canMove = false;
            }
        }
    }
}
