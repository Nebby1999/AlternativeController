using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AC
{
    /// <summary>
    /// Componente el cual asigna 2 puntos a un <see cref="LineRenderer"/>, y renderiza una linea entre estos dos puntos.
    /// </summary>
    [ExecuteAlways]
    [RequireComponent(typeof(LineRenderer))]
    public class LineRendererPointToPoint : MonoBehaviour
    {
        /// <summary>
        /// El line renderer de este componente
        /// </summary>
        public LineRenderer lineRenderer { get; private set; }

        /// <summary>
        /// El punto de comienzo de la linea.
        /// </summary>
        public Transform startPoint => _startPoint;
        [SerializeField, Tooltip("El punto de comienzo de esta linea")] private Transform _startPoint;

        /// <summary>
        /// El punto de termino de la linea
        /// </summary>
        public Transform endPoint => _endPoint;
        [SerializeField, Tooltip("El punto de termino de esta linea.")] private Transform _endPoint;
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
