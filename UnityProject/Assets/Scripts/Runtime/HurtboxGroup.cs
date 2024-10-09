using System.Linq;
using System.Xml;
using System;
using UnityEngine;

namespace AC
{
    /// <summary>
    /// Representa un grupo de <see cref="HurtBox"/> para un cuerpo.
    /// </summary>
    public class HurtBoxGroup : MonoBehaviour
    {
        /// <summary>
        /// Las Hurtboxes de este grupo
        /// </summary>
        public HurtBox[] hurtBoxes => _hurtBoxes;
        [Tooltip("Las hurtboxes en este grupo, hay un ContextMenu para re-llenar este campo automaticamente.")]
        [SerializeField] private HurtBox[] _hurtBoxes = Array.Empty<HurtBox>();

        /// <summary>
        /// El <see cref="HurtBox"/> principal de este objeto
        /// </summary>
        public HurtBox mainHurtBox => _mainHurtBox;

        [Tooltip("El HurtBox principal de este objeto")]
        [SerializeField] private HurtBox _mainHurtBox;

        /// <summary>
        /// Activa o Desactiva las Hurtboxes
        /// </summary>
        /// <param name="active"></param>
        public void SetActiveHurtboxes(bool active)
        {
            for (int i = 0; i < _hurtBoxes.Length; i++)
            {
                _hurtBoxes[i].gameObject.SetActive(active);
            }
        }

        [ContextMenu("Autopopulate array")]
        private void AutoPopulateArray()
        {
            _hurtBoxes = GetComponentsInChildren<HurtBox>();
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
#endif
        }

        private void OnValidate()
        {
            if (_hurtBoxes.Length == 0)
            {
                Debug.LogWarning($"HurtBoxGroup {this} has no HurtBoxes", this);
                return;
            }

            if (!_hurtBoxes.Any(x => x.isBullseye))
            {
                Debug.LogWarning($"HurtBoxGroup {this} has no HurtBox marked as a Bullseye hurtbox!");
                return;
            }
        }
    }
}