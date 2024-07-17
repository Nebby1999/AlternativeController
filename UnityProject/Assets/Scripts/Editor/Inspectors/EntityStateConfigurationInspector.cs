using Nebula.Editor;
using UnityEditor;

namespace AC.Editor.Inspectors
{
    [CustomEditor(typeof(EntityStateConfiguration))]
    public class EntityStateConfigurationInspector : StateConfigurationInspector<EntityStateConfiguration>
    {
        protected override string GetStateTypeToConfigPropertyName()
        {
            return "_stateTypeToConfig";
        }
    }
}