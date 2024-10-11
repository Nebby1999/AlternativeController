using UnityEngine;

namespace Nebula
{
    /// <summary>
    /// <see cref="PropertyAttribute"/> que indica que una propiedad de un <see cref="UnityEngine.Object"/> se deberia usar como el Label en el inspector
    /// </summary>
    public class ValueLabelAttribute : PropertyAttribute
    {
        public string propertyName { get; set; }
        public ValueLabelAttribute(string propertyName = null)
        {
            this.propertyName = propertyName;
        }
    }
}