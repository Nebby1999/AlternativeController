using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Nebula.Editor
{
    public sealed class NebulaSettingsProvider : SettingsProvider
    {
        private NebulaSettings settings;
        private SerializedObject serializedObject;

        [SettingsProvider]
        public static SettingsProvider CreateProvider()
        {
            var keywords = new[] { "Nebula" };
            var settings = NebulaSettings.instance;
            settings.hideFlags = UnityEngine.HideFlags.DontSave | UnityEngine.HideFlags.HideInHierarchy;
            settings.DoSave();
            return new NebulaSettingsProvider("Project/Nebula", SettingsScope.Project, keywords)
            {
                settings = settings,
                serializedObject = new SerializedObject(settings)
            };
        }

        public override void OnActivate(string searchContext, VisualElement rootElement)
        {
            VisualElementTemplateFinder.GetTemplateInstance("NebulaSettingsElement", rootElement);
            rootElement.Bind(serializedObject);
        }

        public override void OnDeactivate()
        {
            base.OnDeactivate();
            Save();
        }

        private void Save()
        {
            serializedObject?.ApplyModifiedProperties();
            if (settings)
                settings.DoSave();
        }

        public NebulaSettingsProvider(string path, SettingsScope scopes, IEnumerable<string> keywords = null) : base(path, scopes, keywords)
        {
        }
    }
}