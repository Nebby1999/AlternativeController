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
    /// Represents a definition for a Resource
    /// </summary>
    [CreateAssetMenu(fileName = "New ResourceDef", menuName = "AC/ResourceDef")]
    public class ResourceDef : NebulaScriptableObject
    {
        [Tooltip("Name of the Resource")]
        public string resourceName;
        [Tooltip("Description of the Resource")]
        public string resourceDescription;
        [Tooltip("Sprite of the resource")]
        public Sprite resourceSprite;
        [Tooltip("The color of the resource")]
        public Color32 resourceColor;

        /// <summary>
        /// Index assigned to this ResourceDef, this is set at Runtime by the <see cref="ResourceCatalog"/>
        /// </summary>
        public ResourceIndex resourceIndex { get; internal set; }
    }

    /// <summary>
    /// Represents an index to a Resource, to get a runtime resource value, obtain said resource's <see cref="ResourceDef"/> using the <see cref="ResourceCatalog"/>
    /// </summary>
    public enum ResourceIndex
    {
        /// <summary>
        /// No resource
        /// </summary>
        None = -1
    };
}