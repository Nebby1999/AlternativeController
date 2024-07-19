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
            public CommonMask[] commonMaskSelector;
            public string filePath;
            public string @namespace;

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