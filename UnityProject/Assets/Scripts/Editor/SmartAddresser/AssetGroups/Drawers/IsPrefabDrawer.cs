using SmartAddresser.Editor.Foundation.CustomDrawers;

namespace AC.Editor.SmartAddresser.AssetFilters.Drawers
{
    [CustomGUIDrawer(typeof(IsPrefab))]
    public class IsPrefabDrawer : GUIDrawer<IsPrefab>
    {
        protected override void GUILayout(IsPrefab target)
        {
        }
    }
}