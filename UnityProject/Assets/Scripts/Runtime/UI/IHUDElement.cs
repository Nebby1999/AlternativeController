using UnityEngine.EventSystems;

namespace AC
{
    public interface IHUDElement
    {
        HUD parentHud { get; set; }
    }
}