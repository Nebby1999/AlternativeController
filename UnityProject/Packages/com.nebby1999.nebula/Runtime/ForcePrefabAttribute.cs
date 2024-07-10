using System;
using UnityEngine;

namespace Nebula
{
    public class ForcePrefabAttribute : PropertyAttribute
    {
        public Type fieldType { get; }
        public ForcePrefabAttribute(Type fieldType)
        {
            this.fieldType = fieldType;
        }
        private ForcePrefabAttribute() { }
    }
}