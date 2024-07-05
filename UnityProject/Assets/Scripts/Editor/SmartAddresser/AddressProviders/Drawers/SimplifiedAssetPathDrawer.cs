using SmartAddresser.Editor.Foundation.CustomDrawers;
using UnityEditor;

namespace AC.Editor.SmartAddresser.AddressProviders.Drawers
{
    [CustomGUIDrawer(typeof(SimplifiedAssetPath))]
    public class SimplifiedAssetPathDrawer : GUIDrawer<SimplifiedAssetPath>
    {
        protected override void GUILayout(SimplifiedAssetPath target)
        {
            EditorGUILayout.LabelField("Simplified Asset Path");
        }
    }
}