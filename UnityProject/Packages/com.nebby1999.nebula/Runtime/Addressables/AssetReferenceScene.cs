#if ADDRESSABLES
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

namespace Nebula
{
    /// <summary>
    /// Una clase de tipo <see cref="AssetReference"/> que se usa para referenciar una Escena.
    /// </summary>
    [System.Serializable]
    public class AssetReferenceScene : AssetReference
    {
        /// <summary>
        /// Crea un nuevo AssetReference usando el guid <paramref name="guid"/>
        /// </summary>
        public AssetReferenceScene(string guid) : base(guid)
        {
        }


        public override bool ValidateAsset(UnityEngine.Object obj)
        {
#if UNITY_EDITOR
            var type = obj.GetType();
            return typeof(UnityEditor.SceneAsset).IsAssignableFrom(type);
#else
            return false;
#endif
        }

        /// <inheritdoc/>
        public override bool ValidateAsset(string path)
        {
#if UNITY_EDITOR
            var type = UnityEditor.AssetDatabase.GetMainAssetTypeAtPath(path);
            return typeof(UnityEditor.SceneAsset).IsAssignableFrom(type);
#else
            return false;
#endif
        }

#if UNITY_EDITOR
        /// <summary>
        /// override especifico del parent editor asset, usado por el editor para representar la escena siendo referenciada.
        /// </summary>
        public new UnityEditor.SceneAsset editorAsset => (UnityEditor.SceneAsset)base.editorAsset;
#endif
    }

}
#endif