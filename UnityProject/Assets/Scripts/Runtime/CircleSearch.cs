using AC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Unity.Collections;
using UnityEngine;
using static AC.CircleSearch;

namespace AC
{
    public class CircleSearch
    {
        public float radius;
        public Vector3 origin;
        public LayerMask candidateMask;
        public bool useTriggers;

        private List<Collider2D> _colliders = new List<Collider2D>();
        private List<Candidate> _candidates;
        public CircleSearch FindCandidates()
        {
            _candidates = new List<Candidate>();
            var contactFilter = new ContactFilter2D
            {
                useTriggers = false,
                useLayerMask = true,
                layerMask = candidateMask
            };

            Physics2D.OverlapCircle(origin, radius, contactFilter, _colliders);
            foreach (var collider in _colliders)
            {
                _candidates.Add(new Candidate
                {
                    collider = collider,
                    distanceSqr = (origin - collider.transform.position).sqrMagnitude,
                    position = collider.transform.position,
                    colliderHurtbox = collider.GetComponent<HurtBox>()
                });
            }
            return this;
        }

        public CircleSearch FilterCandidatesByLOS(LayerMask obstacleMask)
        {
            ThrowIfCandidateListNull();
            for(int i = _candidates.Count - 1; i >= 0; i--)
            {
                var candidate = _candidates[i];
                var hit = Physics2D.Raycast(origin, (origin - candidate.position).normalized, Mathf.Sqrt(candidate.distanceSqr));
                if(hit.collider != candidate.collider)
                {
                    _candidates.RemoveAt(i);
                }
            }
            return this;
        }

        public CircleSearch FilterCandidatesByDistinctHealthComponent()
        {
            if (_candidates == null)
                throw new NullReferenceException("Candidate List not made, call FindCandidates first.");

            List<HealthComponent> distinct = new List<HealthComponent>();
            for (int i = _candidates.Count - 1; i >= 0; i--)
            {
                var candidate = _candidates[i];
                if (!candidate.colliderHurtbox)
                {
                    _candidates.RemoveAt(i);
                    continue;
                }

                var healthComponent = candidate.colliderHurtbox.healthComponent;
                if (!healthComponent)
                {
                    _candidates.RemoveAt(i);
                    continue;
                }

                if (distinct.Contains(healthComponent))
                {
                    _candidates.RemoveAt(i);
                    continue;
                }
                distinct.Add(healthComponent);
            }
            return this;
        }

        public CircleSearch FilterBy(Func<Candidate, bool> predicate)
        {
            ThrowIfCandidateListNull();
            for (int i = _candidates.Count - 1; i >= 0; i--)
            {
                var candidate = _candidates[i];
                if (!predicate(candidate))
                {
                    _candidates.RemoveAt(i);
                }
            }
            return this;
        }

        public CircleSearch OrderByDistance()
        {
            ThrowIfCandidateListNull();
            _candidates = _candidates.OrderBy(k => k.distanceSqr).ToList();
            return this;
        }

        public CircleSearch GetResults(out List<Candidate> results)
        {
            ThrowIfCandidateListNull();
            results = new List<Candidate>();
            for (int i = 0; i < _candidates.Count; i++)
            {
                results.Add(_candidates[i]);
            }
            _candidates.Clear();
            return this;
        }

        private void ThrowIfCandidateListNull()
        {
            if (_candidates == null)
                throw new NullReferenceException("Candidate List not made, call FindCandidates first.");
        }

        public CircleSearch FirstOrDefault(out Candidate candidate)
        {
            candidate = _candidates.FirstOrDefault();
            return this;
        }

        public struct Candidate
        {
            public Collider2D collider;
            public Vector3 position;
            public float distanceSqr;
            public HurtBox colliderHurtbox;
        }
    }
}