using UnityEngine;

namespace AC
{
    public class ResourceDefPreference : MonoBehaviour
    {
        public ResourceDef resourcePreference { get; set; }
        [SerializeField] private ResourceDef _defaultResourcePreference;

        private void Awake()
        {
            resourcePreference = _defaultResourcePreference;
        }
    }
}