using AC;
using Nebula;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Experimental.AI;

namespace EntityStates
{
    public class Wander : BaseAIState
    {
        public static float minTimeBetweenWandering;
        public static float maxTimeBetweenWandering;

        private float _waitTime;
        private float _wanderStopwatch;

        public override void OnEnter()
        {
            base.OnEnter();
            SetNewWaitTime();
            SetWanderTarget();
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (!hasBody)
                return;

            _wanderStopwatch += Time.fixedDeltaTime;
            if (_wanderStopwatch > _waitTime)
            {
                _wanderStopwatch = 0;
                SetNewWaitTime();
                SetWanderTarget();
            }
        }

        private void SetNewWaitTime()
        {
            _waitTime = aiRNG.RangeFloat(minTimeBetweenWandering, maxTimeBetweenWandering);
        }

        private void SetWanderTarget()
        {
            Vector3 randomDirection = Random.insideUnitSphere;
            Vector3 position = randomDirection * aiRNG.RangeFloat(baseAI.visionRange / 2, baseAI.visionRange);
            position += characterBody.transform.position;

            NavMeshHit hit;
            if(NavMesh.SamplePosition(position, out hit, baseAI.visionRange, NavMesh.AllAreas))
            {
#if UNITY_EDITOR
                GlobalGizmos.EnqueueGizmoDrawing(() => Gizmos.DrawSphere(hit.position, 0.5f));
#endif
                NavMeshPath path = new NavMeshPath();
                baseAI.navMeshAgent.CalculatePath(hit.position, path);
                baseAI.navMeshAgent.SetPath(path);
            }
            _waitTime /= 2;
            /*randomDirection += characterBody.transform.position;
            NavMeshHit hit;
            Vector3 finalPosition = Vector3.zero;
            if (NavMesh.SamplePosition(randomDirection, out hit, baseAI.visionRange, NavMesh.AllAreas))
            {
                NavMeshPath path = new NavMeshPath();
                baseAI.navMeshAgent.CalculatePath(hit.position, path);
                baseAI.navMeshAgent.SetPath(path);
            }
            _waitTime /= 2;*/
            return;
        }
    }
}
