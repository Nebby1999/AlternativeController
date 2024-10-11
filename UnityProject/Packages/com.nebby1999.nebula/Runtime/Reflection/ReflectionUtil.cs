using System;
using System.Linq;
using System.Reflection;

namespace Nebula
{
    /// <summary>
    /// Contiene metodos utilizados para la Refleccion
    /// </summary>
    public static class ReflectionUtil
    {
        /// <summary>
        /// Shortcut para ocupar todo valor posible de <see cref="BindingFlags"/>
        /// </summary>
        public static readonly BindingFlags all = (BindingFlags)(-1);

        /// <summary>
        /// Consigue todos los Types dentro de assembly <paramref name="assembly"/>. Maneja la <see cref="ReflectionTypeLoadException"/> devolviendo los types que si se lograron cargar.
        /// </summary>
        public static Type[] GetTypesSafe(this Assembly assembly)
        {
            Type[] types = null;
            try
            {
                types = assembly.GetTypes();
            }
            catch(ReflectionTypeLoadException ex)
            {
                types = ex.Types.Where(t => t != null).ToArray();
            }
            return types;
        }
    }
}