using System;
using System.Collections.Generic;
using UnityEngine;

namespace Nebula
{
#if UNITY_EDITOR
    /// <summary>
    /// Clase exlcusiva del editor, te permite dibujar <see cref="Gizmos"/> desde cualquier clase, sin necesidad de usar un componente
    /// </summary>
    public class GlobalGizmos : SingletonMonoBehaviour<GlobalGizmos>
    {
        protected override bool destroyIfDuplicate => true;
        private static List<DrawRequest> _drawRequests = new List<DrawRequest>();
        private static List<int> _finishedRequests = new List<int>();

        /// <summary>
        /// Llama <paramref name="action"/> durante los segundos especificados en <paramref name="duration"/>.
        /// </summary>
        /// <param name="action">La accion en si, deberia llamar a metodos dentro de <see cref="Gizmos"/></param>
        /// <param name="duration">La duracion de los dibujos.</param>
        public static void EnqueueGizmoDrawing(Action action, float duration = 60)
        {
            if(!instance)
            {
                if(Application.isPlaying)
                {
                    var go = new GameObject();
                    go.name = "DEBUG_GlobalGizmos";
                    go.AddComponent<GlobalGizmos>();
                    DontDestroyOnLoad(go);
                }
                else
                {
                    var go = new GameObject();
                    go.name = "DEBUG_GlobalGizmmos";
                    go.AddComponent<GlobalGizmos>();
                    go.hideFlags = HideFlags.NotEditable | HideFlags.DontSaveInEditor | HideFlags.DontSaveInBuild;
                }
            }

            _drawRequests.Add(new DrawRequest
            {
                gizmoDrawingMethod = action,
                remainingDrawCalls = Mathf.RoundToInt(duration * 60)
            });
        }

        private void OnDrawGizmos()
        {
            _finishedRequests.Clear();
            for (int i = 0; i < _drawRequests.Count; i++)
            {
                var request = _drawRequests[i];
                request.gizmoDrawingMethod();
                request.remainingDrawCalls--;
                _drawRequests[i] = request;
                if (request.remainingDrawCalls <= 0)
                {
                    _finishedRequests.Add(i);
                }
            }

            foreach (var requestIndex in _finishedRequests)
            {
                _drawRequests.RemoveAt(requestIndex);
            }
        }
        private struct DrawRequest
        {
            public Action gizmoDrawingMethod;
            public int remainingDrawCalls;
        }
    }
#endif
}