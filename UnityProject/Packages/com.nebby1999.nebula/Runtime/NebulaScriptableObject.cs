using System;
using UnityEngine;

namespace Nebula
{
    /// <summary>
    /// ScriptableObject base para todo <see cref="ScriptableObject"/> dentro de un juego.
    /// <br>Evita crear memoria basura cuando se accede <see cref="UnityEngine.Object.name"/> usando <see cref="cachedName"/></br>
    /// </summary>
    public abstract class NebulaScriptableObject : ScriptableObject
    {
        private const string MESSAGE = "Retrieving the name from the engine causes allocations on runtime, use cachedName instead, if retrieving the value from the engine is absolutely necesary, cast to ScriptableObject first.";

        /// <summary>
        /// Consigue el nombre de este objeto usando el string en el cache
        /// </summary>
        public string cachedName
        {
            get
            {
                _cachedName ??= base.name;
                return _cachedName;
            }
            set
            {
                base.name = value;
                _cachedName = value;
            }
        }
        private string _cachedName = null;

        /// <summary>
        /// Conseguir el nombre del objeto desde el engine de C++ causa alocaciones inecesarias, utiliza <see cref="cachedName"/>. Si conseguir el valor de C++ es necesario en absoluto, castea este objeto a <see cref="ScriptableObject"/> primero.
        /// </summary>
        [Obsolete(MESSAGE, true)]
        new public string name { get => throw new System.NotSupportedException(MESSAGE); set => throw new System.NotSupportedException(MESSAGE); }

        /// <summary>
        /// Asigna <see cref="_cachedName"/> al nombre del objeto
        /// </summary>
        protected virtual void Awake()
        {
            _cachedName = base.name;
        }
    }
}