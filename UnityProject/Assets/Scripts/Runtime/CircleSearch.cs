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
    /// <summary>
    /// Clase usada para encontrar objetos varios con colliders dentro del mundo.
    /// </summary>
    public class CircleSearch
    {
        /// <summary>
        /// El radio de busqueda
        /// </summary>
        public float radius;

        /// <summary>
        /// El punto de origen de la busqueda
        /// </summary>
        public Vector3 origin;

        /// <summary>
        /// Que layers se deberian usar en el proceso de busqueda
        /// </summary>
        public LayerMask candidateMask;

        /// <summary>
        /// Si el proceso de busqueda ocupa colliders tipo Trigger.
        /// </summary>
        public bool useTriggers;

        /// <summary>
        /// El objeto ejecutando la busqueda
        /// </summary>
        public GameObject searcher;

        private List<Collider2D> _colliders = new List<Collider2D>();
        private List<Candidate> _candidates;

        /// <summary>
        /// Metodo conectable que encuentra los candidatos base para luego filtrarlos
        /// </summary>
        /// <returns>La instancia actual de CircleSearch</returns>
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
                var hb = collider.GetComponent<HurtBox>();
                _candidates.Add(new Candidate
                {
                    collider = collider,
                    distanceSqr = (origin - collider.transform.position).sqrMagnitude,
                    position = collider.transform.position,
                    colliderHurtbox = hb,
                    entityObject = hb ? hb.healthComponent ? hb.healthComponent.gameObject : collider.gameObject : collider.gameObject
                });
            }
            return this;
        }

        /// <summary>
        /// Filtra todo <see cref="Candidate"/> que no tenga el componente <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T">El tipo de componente que los candidatos deberian tener</typeparam>
        /// <returns>La instancia actual de CircleSearch</returns>
        public CircleSearch FilterCandidatesByComponent<T>()
        {
            return FilterCandidatesByComponent(typeof(T));
        }

        /// <summary>
        /// Filtra todo <see cref="Candidate"/> que no tenga el componente <paramref name="componentType"/>
        /// </summary>
        /// <param name="componentType">El tipo de componente que los candidatos deberian tener</param>
        /// <returns>La instancia actual de CircleSearch</returns>
        public CircleSearch FilterCandidatesByComponent(Type componentType)
        {
            ThrowIfCandidateListNull();
            for (int i = _candidates.Count - 1; i >= 0; i--)
            {
                var candidate = _candidates[i];
                if (!candidate.collider.TryGetComponent(componentType, out var component))
                {
                    _candidates.RemoveAt(i);
                    continue;
                }
                candidate.componentChosenDuringFilterByComponent = component;
                _candidates[i] = candidate;
            }
            return this;
        }

        /// <summary>
        /// Filtra todo <see cref="Candidate"/> que no tenga una linea de vision directa entre <see cref="searcher"/> y el candidato
        /// </summary>
        /// <param name="obstacleMask">Una mascara que identifica objetos que son obstaculos</param>
        /// <returns>La instancia actual de CircleSearch</returns>
        public CircleSearch FilterCandidatesByLOS(LayerMask obstacleMask)
        {
            ThrowIfCandidateListNull();
            for(int i = _candidates.Count - 1; i >= 0; i--)
            {
                var candidate = _candidates[i];
                var hit = Physics2D.Raycast(origin, (origin - candidate.position).normalized, Mathf.Sqrt(candidate.distanceSqr), obstacleMask);
                if(hit.collider != candidate.collider)
                {
                    _candidates.RemoveAt(i);
                }
            }
            return this;
        }

        /// <summary>
        /// Filtra todo <see cref="Candidate"/> que no tenga un <see cref="HurtBox"/> o un <see cref="HealthComponent"/>
        /// </summary>
        /// <returns>La instancia actual de CircleSearch</returns>
        public CircleSearch FilterCandidatesByDistinctHealthComponent()
        {
            ThrowIfCandidateListNull();

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

        /// <summary>
        /// Filtra todo <see cref="Candidate"/> que no tenga el tag <paramref name="tagName"/>
        /// </summary>
        /// <param name="tagName">El tag para filtrar</param>
        /// <returns>La instancia actual de CircleSearch</returns>
        public CircleSearch FilterCandidatesByTag(string tagName)
        {
            ThrowIfCandidateListNull();

            for (int i = _candidates.Count - 1; i >= 0; i--)
            {
                var candidate = _candidates[i];
                if (!candidate.entityObject.CompareTag(tagName))
                {
                    _candidates.RemoveAt(i);
                }
            }
            return this;
        }

        /// <summary>
        /// Filtra todo <see cref="Candidate"/> que el <paramref name="predicate"/> retorne falso
        /// </summary>
        /// <param name="predicate">Una funcion que devuelve True si el candidato para el filtro, o False si no pasa el filtro.</param>
        /// <returns>La instancia actual de CircleSearch</returns>
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

        /// <summary>
        /// Ordena los <see cref="Candidate"/> por distancia
        /// </summary>
        /// <returns>La instancia actual de CircleSearch</returns>
        public CircleSearch OrderByDistance()
        {
            ThrowIfCandidateListNull();
            _candidates = _candidates.OrderBy(k => k.distanceSqr).ToList();
            return this;
        }

        /// <summary>
        /// Filtra <see cref="searcher"/> de la lista de <see cref="Candidate"/>
        /// </summary>
        /// <returns>La instancia actual de CircleSearch</returns>
        public CircleSearch FilterSearcher()
        {
            ThrowIfCandidateListNull();
            for(int i = _candidates.Count - 1; i >= 0; i--)
            {
                var candidate = _candidates[i];
                if(candidate.entityObject  == searcher)
                {
                    _candidates.RemoveAt(i);
                }
            }
            return this;
        }

        /// <summary>
        /// Obtiene los resultados actuales del CircleSearch
        /// </summary>
        /// <param name="results">Los candidatos actuales</param>
        /// <returns>La instancia actual de CircleSearch</returns>
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

        /// <summary>
        /// Retorna el primer candidato en la lista
        /// </summary>
        /// <param name="candidate">El primer candidato de la lista</param>
        /// <returns>La instancia actual de CircleSearch</returns>
        public CircleSearch FirstOrDefault(out Candidate candidate)
        {
            ThrowIfCandidateListNull();
            candidate = _candidates.FirstOrDefault();
            return this;
        }

        /// <summary>
        /// Representa un Candidato en la busqueda
        /// </summary>
        public struct Candidate
        {
            /// <summary>
            /// El collider que encontramos
            /// </summary>
            public Collider2D collider;
            /// <summary>
            /// El objeto "Entidad", este tipo de objeto usualmente es el objeto principal del candidato. Por ejemplo, si <see cref="colliderHurtbox"/> no es null, entonces <see cref="entityObject"/> es el GameObject con un <see cref="HealthComponent"/>
            /// </summary>
            public GameObject entityObject;
            /// <summary>
            /// La psoicion del candidato
            /// </summary>
            public Vector3 position;
            /// <summary>
            /// La distancia al cuadrado entre el buscador y el candidato
            /// </summary>
            public float distanceSqr;
            /// <summary>
            /// La hurtbox encontrada en el <see cref="collider"/>, puede ser null
            /// </summary>
            public HurtBox colliderHurtbox;

#nullable enable
            /// <summary>
            /// El componente escojido durante una llamada a <see cref="FilterCandidatesByComponent{T}()"/> o <see cref="FilterCandidatesByComponent(Type)"/>
            /// </summary>
            public Component? componentChosenDuringFilterByComponent;
#nullable disable
        }
    }
}