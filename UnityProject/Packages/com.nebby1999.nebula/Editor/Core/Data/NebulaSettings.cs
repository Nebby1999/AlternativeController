using Nebula.Editor.CodeGenerators;
using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Nebula.Editor
{
    [FilePath("ProjectSettings/NebulaSettings.asset", FilePathAttribute.Location.ProjectFolder)]
    public sealed class NebulaSettings : ScriptableSingleton<NebulaSettings>
    {
        public InputActionGUIDData[] inputActionGuidDatas = Array.Empty<InputActionGUIDData>();
        public bool createLayerIndexStruct = true;
        public LayerIndexData layerIndexData;
        public bool createGameTagsStruct = true;
        public GameTagsData gameTagsData;


        internal void DoSave()
        {
            Save(false);
        }

        [MenuItem("Tools/Nebula/Generate Layer Index")]
        private static void MenuItem_GenerateLayerIndexStruct()
        {
            instance.GenerateLayerIndexStruct();
        }

        [MenuItem("Tools/Nebula/Generate Layer Index", true)]
        private static bool MenuItemValidate_GenerateLayerIndexStruct()
        {
            return instance.createLayerIndexStruct;
        }

        [MenuItem("Tools/Nebula/Generate Game Tags Struct")]
        private static void MenuItem_GenerateGameTagsStruct()
        {
            instance.GenerateGameTagsStruct();
        }

        [MenuItem("Tools/Nebula/Generate Game Tags Struct", true)]
        private static bool MenuItemValidate_GenerateGameTagsStruct()
        {
            return instance.createGameTagsStruct;
        }


        [MenuItem("Tools/Nebula/Generate Input Action GUID Classes")]
        private static void MenuItem_GenerateInputActionGUIDClasses()
        {
            instance.GenerateInputActionGUIDClasses();
        }

        [MenuItem("Tools/Nebula/Generate Input Action GUID Classes", true)]
        private static bool MenuItemValidate_GenerateInputActionGUIDClasses()
        {
            return instance.inputActionGuidDatas.Length != 0;
        }

        internal void GenerateLayerIndexStruct()
        {
            if (createLayerIndexStruct)
            {
                LayerIndexCodeGenerator.GenerateCode(layerIndexData);
            }
        }

        internal void GenerateInputActionGUIDClasses()
        {
            for(int i = 0; i < inputActionGuidDatas.Length; i++)
            {
                InputActionGUIDCodeGenerator.GenerateCode(inputActionGuidDatas[i]);
            }
        }

        internal void GenerateGameTagsStruct()
        {
            if(createGameTagsStruct)
            {
                GameTagsCodeGenerator.GenerateCode(gameTagsData);
            }
        }

        [Serializable]
        public struct LayerIndexData
        {
            public bool is2D;
            public CommonMask[] commonMaskSelector;
            [FilePickerPath(defaultName = "LayerIndex", extension = "cs", title = "Location for generated C# file")]
            public string filePath;
            public string nameSpace;

            [Serializable]
            public struct CommonMask
            {
                public string comment;
                public string maskName;
                public LayerMask layerMask;
            }
        }

        [Serializable]
        public struct GameTagsData
        {
            [FilePickerPath(defaultName = "GameTags", extension = "cs", title = "Location for generated C# file")]
            public string filePath;
            public string nameSpace;
        }

        [Serializable]
        public struct InputActionGUIDData
        {
            public InputActionAsset inputActionAsset;
            [FilePickerPath(defaultName = "InputActionGUIDS", extension = "cs", title = "Location for generated C# file")]
            public string filePath;
            public string nameSpace;
        }
    }
}