using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AC
{
    [ExecuteAlways]
    [RequireComponent(typeof(LineRenderer))]
    public class LineRendererPointToPoint : MonoBehaviour
    {
        public LineRenderer lineRenderer { get; private set; }
        public Transform startPoint => _startPoint;
        [SerializeField] private Transform _startPoint;
        public Transform endPoint => _endPoint;
        [SerializeField] private Transform _endPoint;
        private void Awake()
        {
            lineRenderer = GetComponent<LineRenderer>();
        }

        private void Start()
        {
            lineRenderer.positionCount = 2;
        }

        private void Update()
        {
            lineRenderer.SetPosition(0, startPoint ? startPoint.position : Vector3.zero);
            lineRenderer.SetPosition(1, endPoint ? endPoint.position : Vector3.right);
        }
    }
}
