using System;
using UnityEngine;

namespace Nebula
{
    /// <summary>
    /// PropertyAttribute que indica que un <see cref="GameObject"/> o <see cref="Component"/> debe venir de un Prefab.
    /// </summary>
    public class ForcePrefabAttribute : PropertyAttribute
    {
        public ForcePrefabAttribute() { }
    }
}