using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AC
{

    [Serializable]
    public class TargetPriorityManager
    {
        [SerializeField] private List<GameObject> _resourceSources;
        [SerializeField] private List<GameObject> _bases;
        [SerializeField] private List<GameObject> _players;
        private Transform _transform;
        public void Initialize(Transform transform)
        {
            _transform = transform;
            GetObjects();
        }
        private void GetObjects()
        {
            var players = GameObject.FindObjectsOfType<Vehicle_OLD>().ToList();
            var resources = GameObject.FindObjectsOfType<MineralOre>().ToList();
            var minerals = GameObject.FindObjectsOfType<Mineral>().ToList();
            foreach (var item in players)
            {
                _players.Add(item.gameObject);
            }
            foreach (var item in resources)
            {
                _resourceSources.Add(item.gameObject);
            }
            foreach (var item in minerals)
            {
                _resourceSources.Add(item.gameObject);
            }
        }
        public Transform GetHighestPriorityTarget()
        {
            List<GameObject> allTargets = new List<GameObject>();
            allTargets.AddRange(_resourceSources);
            allTargets.AddRange(_bases);
            allTargets.AddRange(_players);

            GameObject highestPriorityTarget = null;
            float highestPriority = 0;

            foreach (GameObject target in allTargets)
            {
                float distancePriority = CalculateDistancePriority(target);
                float typePriority = CalculateTypePriority(target);

                float totalPriority = distancePriority + typePriority;

                if (totalPriority > highestPriority)
                {
                    highestPriority = totalPriority;
                    highestPriorityTarget = target;
                }
            }

            return highestPriorityTarget?.transform;
        }

        private float CalculateDistancePriority(GameObject target)
        {
            if (target == null)
            {
                return 0;
            }

            float distance = Vector3.Distance(_transform.position, target.transform.position);
            return 1 / (distance + 1);
        }

        private float CalculateTypePriority(GameObject target)
        {
            if (_resourceSources.Contains(target))
            {
                return 0.05f;
            }
            else if (_bases.Contains(target))
            {
                return 0.2f;
            }
            else if (_players.Contains(target))
            {
                return 0.2f;
            }

            return 0;
        }
    }
}
