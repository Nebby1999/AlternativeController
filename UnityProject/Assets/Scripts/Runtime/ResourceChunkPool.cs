using Nebula;
using UnityEngine;

namespace AC
{
    public class ResourceChunkPool : MonoBehaviour
    {
        [ForcePrefab]
        public ResourceChunk prefab;
        public int initialPoolSize = 10;
    }
}