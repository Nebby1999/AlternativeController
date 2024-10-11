using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Nebula
{
    /// <summary>
    /// Clase utilizada para encontrar de manera facil a Transforms que son hijos de este componente.
    /// </summary>
    public class ChildLocator : MonoBehaviour
    {
        /// <summary>
        /// Indica un par de Transform y su nombre
        /// </summary>
        [Serializable]
        public struct NameTransformPair
        {
            /// <summary>
            /// El nombre de esta entrada
            /// </summary>
            public string name;

            /// <summary>
            /// El Transform relacionado
            /// </summary>
            public Transform tiedTransform;
        }

        /// <summary>
        /// Las entradas en este ChildLocator
        /// </summary>
        public NameTransformPair[] entries => _transformPairs;
        [SerializeField]
        [Tooltip("Las entradas en este child locator")]
        private NameTransformPair[] _transformPairs;

        /// <summary>
        /// Encuentra un transform de nombre <paramref name="name"/> y lo devuelve.
        /// </summary>
        public Transform FindChild(string name) => FindChild(FindChildIndex(name));

        /// <summary>
        /// Encuentra el transform en el indice <paramref name="childIndex"/> y lo devleuve.
        /// </summary>
        public Transform FindChild(int childIndex) => _transformPairs[childIndex].tiedTransform;

        /// <summary>
        /// Encuentra el GameObject cuyo nombre de transform es <paramref name="name"/> y lo devuelve.
        /// </summary>
        public GameObject FindChildGameObject(string name) => FindChildGameObject(FindChildIndex(name));

        /// <summary>
        /// Encuentra el GameObject cuyo transform es el indice <paramref name="childIndex"/>
        /// </summary>
        public GameObject FindChildGameObject(int childIndex) => _transformPairs[childIndex].tiedTransform.gameObject;

        /// <summary>
        /// Llama <see cref="Component.GetComponent{T}()"/> con el valor <typeparamref name="T"/> en el GameObject cuyo transform tine ede nombre <paramref name="childName"/>
        /// </summary>
        public T FindComponentInChild<T>(string childName) where T : Component
        {
            return FindComponentInChild<T>(FindChildIndex(childName));
        }


        /// <summary>
        /// Llama <see cref="Component.GetComponent{T}()"/> con el valor <typeparamref name="T"/> en el GameObject cuyo transform es el indice <paramref name="childIndex"/>
        /// </summary>
        public T FindComponentInChild<T>(int childIndex) where T : Component
        {
            return _transformPairs[childIndex].tiedTransform.GetComponent<T>();
        }

        /// <summary>
        /// Encuentra el nombre del transform en el indice <paramref name="childIndex"/>
        /// </summary>
        public string FindChildName(int childIndex) => childIndex < _transformPairs.Length ? _transformPairs[childIndex].name : null;

        /// <summary>
        /// Encuentra el indice del tranrsform especificado en <paramref name="childTransform"/>
        /// </summary>
        public int FindChildIndex(Transform childTransform)
        {
            for (int i = 0; i < _transformPairs.Length; i++)
            {
                if (_transformPairs[i].tiedTransform == childTransform)
                {
                    return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// Encuentra el indice de un transform cuyo nombre es igual a <paramref name="name"/>
        /// </summary>
        public int FindChildIndex(string name)
        {
            for (int i = 0; i < _transformPairs.Length; i++)
            {
                if (_transformPairs[i].name.Equals(name, StringComparison.OrdinalIgnoreCase))
                {
                    return i;
                }
            }
            return -1;
        }
    }
}