using UnityEngine.EventSystems;

namespace AC
{
    /// <summary>
    /// Interfaz que representa un elemento de HUD
    /// </summary>
    public interface IHUDElement
    {
        /// <summary>
        /// El HUD dueño de este elemento
        /// </summary>
        HUD parentHud { get; set; }
    }
}