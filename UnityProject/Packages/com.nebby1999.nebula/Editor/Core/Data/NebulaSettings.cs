using Nebula.Editor.CodeGenerators;
using System;
using UnityEditor;
using UnityEngine;

namespace Nebula.Editor
{
    [FilePath("ProjectSettings/NebulaSettings.asset", FilePathAttribute.Location.ProjectFolder)]
    public sealed class NebulaSettings : ScriptableSingleton<NebulaSettings>
    {
        public bool createLayerIndexStruct = true;
        public LayerIndexData layerIndexData;

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

        internal void GenerateLayerIndexStruct()
        {
            if (createLayerIndexStruct)
            {
                LayerIndexCodeGenerator.GenerateCode(layerIndexData);
            }
        }

        [Serializable]
        public struct LayerIndexData
        {
            public bool is2D;
            public CommonMask[] commonMaskSelector;
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
    }
}