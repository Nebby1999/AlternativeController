using UnityEngine;

namespace AC
{
    /// <summary>
    /// Representa una preferencia de Recurso.
    /// </summary>
    public class ResourceDefPreference : MonoBehaviour
    {
        /// <summary>
        /// La preferencia de recurso
        /// </summary>
        public ResourceDef resourcePreference { get; set; }
        [SerializeField, Tooltip("El valor por defecto de preferencia.")] private ResourceDef _defaultResourcePreference;

        private void Awake()
        {
            resourcePreference = _defaultResourcePreference;
        }
    }
}