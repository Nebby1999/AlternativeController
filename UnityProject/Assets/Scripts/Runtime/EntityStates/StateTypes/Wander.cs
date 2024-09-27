using AC;
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
            Vector3 randomDirection = Random.insideUnitSphere * 3f;
            randomDirection += characterBody.transform.position;
            NavMeshHit hit;
            Vector3 finalPosition = Vector3.zero;
            if (NavMesh.SamplePosition(randomDirection, out hit, 3f, NavMesh.AllAreas))
            {
                baseAI.currentTarget = new BaseAI.Target(hit.position);
            }
            _waitTime /= 2;
            return;
        }
    }
}
