using UnityEngine.EventSystems;

namespace AC
{
    /// <summary>
    /// Interfaz que representa un elemento de HUD
    /// </summary>
    public interface IHUDElement
    {
        /// <summary>
        /// El HUD due�o de este elemento
        /// </summary>
        HUD parentHud { get; set; }
    }
}