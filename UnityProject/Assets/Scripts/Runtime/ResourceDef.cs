using Nebula;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AC
{
    /// <summary>
    /// Representa la definicion de un Recurso
    /// </summary>
    [CreateAssetMenu(fileName = "New ResourceDef", menuName = "AC/ResourceDef")]
    public class ResourceDef : NebulaScriptableObject
    {
        [Tooltip("El nombre del recurso")]
        public string resourceName;
        [Tooltip("Una descripcion del recurso")]
        public string resourceDescription;
        [Tooltip("Un Sprite del recurso")]
        public Sprite resourceSprite;
        [Tooltip("El color del recurso")]
        public Color32 resourceColor;

        /// <summary>
        /// Un indice unico asignado a este ResourceDef, este valor es colocado por el <see cref="ResourceCatalog"/>
        /// </summary>
        public ResourceIndex resourceIndex { get; internal set; }
    }

    /// <summary>
    /// Representa un indice de recurso, para conseguir un valor a nivel runtime, obten el <see cref="ResourceDef"/> del recurso usando el <see cref="ResourceCatalog"/>
    /// </summary>
    public enum ResourceIndex
    {
        /// <summary>
        /// Un recurso invalido
        /// </summary>
        None = -1
    };
}