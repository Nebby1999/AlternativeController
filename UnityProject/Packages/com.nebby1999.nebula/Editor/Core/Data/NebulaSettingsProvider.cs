using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Nebula.Editor
{
    public sealed class NebulaSettingsProvider : SettingsProvider
    {
        private NebulaSettings _settings;
        private SerializedObject _serializedObject;

        private SerializedProperty _filePathProperty;
        private TextField _filePathField;
        [SettingsProvider]
        public static SettingsProvider CreateProvider()
        {
            var keywords = new[] { "Nebula" };
            var settings = NebulaSettings.instance;
            settings.hideFlags = UnityEngine.HideFlags.DontSave | UnityEngine.HideFlags.HideInHierarchy;
            settings.DoSave();
            return new NebulaSettingsProvider("Project/Nebula", SettingsScope.Project, keywords)
            {
                _settings = settings,
                _serializedObject = new SerializedObject(settings)
            };
        }

        public override void OnActivate(string searchContext, VisualElement rootElement)
        {
            _filePathProperty = _serializedObject.FindProperty(nameof(NebulaSettings.layerIndexData) + "." + (nameof(NebulaSettings.LayerIndexData.filePath)));
            var templateRoot = VisualElementTemplateFinder.GetTemplateInstance("NebulaSettingsElement", rootElement);
            rootElement.Bind(_serializedObject);
            InitializeElement(templateRoot);
        }

        private void InitializeElement(VisualElement templateRoot)
        {
            _filePathField = templateRoot.Q<TextField>("FilePath");
            var enableLauyerIndexGenerationToggle = templateRoot.Q<Toggle>("EnableLayerIndexGeneration");
            var regenerateLayerIndexButton = templateRoot.Q<Button>("RegenerateLayerIndexStructButton");
            var layerIndexContents = templateRoot.Q<VisualElement>("LayerIndexContents");
            enableLauyerIndexGenerationToggle.RegisterValueChangedCallback((evt) =>
            {
                var val = evt.newValue;
                regenerateLayerIndexButton.SetEnabled(val);
                layerIndexContents.SetEnabled(val);
            });
            regenerateLayerIndexButton.SetEnabled(enableLauyerIndexGenerationToggle.value);
            regenerateLayerIndexButton.clicked += RegenerateLayerIndexButton_clicked;

            layerIndexContents.SetEnabled(enableLauyerIndexGenerationToggle.value);
            var selectFolderbutton = templateRoot.Q<Button>("SelectFolderButton");
            selectFolderbutton.clicked += SelectFolderbutton_clicked;
        }

        private void SelectFolderbutton_clicked()
        {
            var fileName = EditorUtility.SaveFilePanel("Location for generated C# file", Application.dataPath, "LayerIndex", "cs");
            if(!string.IsNullOrEmpty(fileName))
            {
                if(fileName.StartsWith(Application.dataPath))
                {
                    fileName = "Assets/" + fileName.Substring(Application.dataPath.Length + 1);
                }
                _filePathField.value = fileName;
            }
        }

        private void RegenerateLayerIndexButton_clicked()
        {
            _settings.GenerateLayerIndexStruct();
        }

        public override void OnDeactivate()
        {
            base.OnDeactivate();
            Save();
        }

        private void Save()
        {
            _serializedObject?.ApplyModifiedProperties();
            if (_settings)
                _settings.DoSave();
        }

        public NebulaSettingsProvider(string path, SettingsScope scopes, IEnumerable<string> keywords = null) : base(path, scopes, keywords)
        {
        }
    }
}