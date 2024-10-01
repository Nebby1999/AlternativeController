using System.Linq;
using System.Xml;
using System;
using UnityEngine;

namespace AC
{
    public class HurtBoxGroup : MonoBehaviour
    {
        public HurtBox[] hurtBoxes => _hurtBoxes;
        [SerializeField] private HurtBox[] _hurtBoxes = Array.Empty<HurtBox>();
        public HurtBox mainHurtBox => _mainHurtBox;

        [SerializeField] private HurtBox _mainHurtBox;

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