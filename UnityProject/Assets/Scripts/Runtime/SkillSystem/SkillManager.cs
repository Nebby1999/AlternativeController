using AC;
using Nebula;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AC
{
    public enum SkillSlot
    {
        None,
        Primary,
        Secondary
    }
    public class SkillManager : MonoBehaviour
    {
        public GenericSkill Primary => _primary;
        [SerializeField, ValueLabel("genericSkillName")] private GenericSkill _primary;
        public GenericSkill Secondary => _secondary;
        [SerializeField, ValueLabel("genericSkillName")] private GenericSkill _secondary;

        private GenericSkill[] _allSkills = Array.Empty<GenericSkill>();

        private void Awake()
        {
            _allSkills = GetComponents<GenericSkill>();
        }

        public SkillSlot FindSkillSlot(GenericSkill genericSkill)
        {
            if (!genericSkill)
                return SkillSlot.None;

            if (genericSkill == _primary)
                return SkillSlot.Primary;

            if (genericSkill == _secondary)
                return SkillSlot.Secondary;

            return SkillSlot.None;
        }

        public int GetSkillIndex(GenericSkill skill)
        {
            return Array.IndexOf(_allSkills, skill);
        }

        public GenericSkill GetSkillByIndex(int index)
        {
            return ArrayUtils.GetSafe(ref _allSkills, index);
        }

        public GenericSkill GetSkillBySkillSlot(SkillSlot slot)
        {
            switch (slot)
            {
                case SkillSlot.Primary: return Primary;
                case SkillSlot.Secondary: return Secondary;
            }
            return null;
        }
        public GenericSkill GetSkill(string name)
        {
            for (int i = 0; i < _allSkills.Length; i++)
            {
                var skill = _allSkills[i];
                if (skill.genericSkillName.Equals(name, StringComparison.OrdinalIgnoreCase))
                    return skill;
            }
            return null;
        }

    }
}