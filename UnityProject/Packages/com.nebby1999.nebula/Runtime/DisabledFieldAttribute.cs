using System;
using UnityEngine;

namespace Nebula
{
    /// <summary>
    /// Indica que un field en un componente deberia ser dibujado "Desactivado", no permitiendo que este field sea modificado en el inspector.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class DisabledFieldAttribute : PropertyAttribute
    {

    }
}