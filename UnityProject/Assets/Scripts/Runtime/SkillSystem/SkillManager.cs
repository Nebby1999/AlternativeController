using AC;
using Nebula;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AC
{
    /// <summary>
    /// Enum que representa slots de habilidades, y por ende tipos de habilidades
    /// </summary>
    public enum SkillSlot
    {
        /// <summary>
        /// Ningun tipo de habilidad.
        /// </summary>
        None,
        /// <summary>
        /// Habilidad primaria
        /// </summary>
        Primary,
        /// <summary>
        /// Habilidad secundaria
        /// </summary>
        Secondary,

        /// <summary>
        /// Habilidad especial
        /// </summary>
        Special
    }

    /// <summary>
    /// Un SkillManager es una Fachada que se utiliza para obtener distinos <see cref="GenericSkill"/> dentro de un objeto.
    /// 
    /// <para>Un objeto puede tener multipler <see cref="GenericSkill"/> pero solo un SkillManager.</para>
    /// </summary>
    [DisallowMultipleComponent]
    public class SkillManager : MonoBehaviour
    {
        /// <summary>
        /// La habilidad primaria en este Skill Manager
        /// </summary>
        public GenericSkill primary => _primary;
        [Tooltip("La habilidad primaria en este Skill Manager")]
        [SerializeField, ValueLabel("genericSkillName")] private GenericSkill _primary;

        /// <summary>
        /// La habilidad secundaria en este Skill Manager
        /// </summary>
        public GenericSkill secondary => _secondary;
        [Tooltip("La habilidad secundaria en este Skill Manager")]
        [SerializeField, ValueLabel("genericSkillName")] private GenericSkill _secondary;

        /// <summary>
        /// La Habilidad especial en este Skill Manager
        /// </summary>
        public GenericSkill special => _special;
        [Tooltip("La habilidad secundaria en este skill manager")]
        [SerializeField, ValueLabel("genericSkillName")] private GenericSkill _special;

        private GenericSkill[] _allSkills = Array.Empty<GenericSkill>();

        private void Awake()
        {
            _allSkills = GetComponents<GenericSkill>();
        }

        /// <summary>
        /// Encuentra el <see cref="SkillSlot"/> asociado a <paramref name="genericSkill"/>
        /// </summary>
        /// <param name="genericSkill">El <see cref="GenericSkill"/> a usar para encontrar su <see cref="SkillSlot"/></param>
        /// <returns>El skill slot de la generic skill, puede retornar <see cref="SkillSlot.None"/> si <paramref name="genericSkill"/> es null o si no esta asignada a ningun slot especifico</returns>
        public SkillSlot FindSkillSlot(GenericSkill genericSkill)
        {
            if (!genericSkill)
                return SkillSlot.None;

            if (genericSkill == _primary)
                return SkillSlot.Primary;

            if (genericSkill == _secondary)
                return SkillSlot.Secondary;

            if (genericSkill == _special)
                return SkillSlot.Special;

            return SkillSlot.None;
        }

        /// <summary>
        /// Obtiene el indice de la habilidad especificada
        /// </summary>
        /// <param name="skill">La habilidad para conseguir su indice</param>
        /// <returns>El indice de la habilidad</returns>
        public int GetSkillIndex(GenericSkill skill)
        {
            return Array.IndexOf(_allSkills, skill);
        }

        /// <summary>
        /// Obtiene la habilidad en el indice <paramref name="index"/>
        /// </summary>
        /// <param name="index">El indice a usar para obtener la habilidad</param>
        /// <returns>La Habilidad en si</returns>
        public GenericSkill GetSkillByIndex(int index)
        {
            return ArrayUtils.GetSafe(ref _allSkills, index);
        }

        /// <summary>
        /// Obtiene el <see cref="GenericSkill"/> asociado a <paramref name="slot"/>
        /// </summary>
        /// <param name="slot">El slot a usar</param>
        /// <returns>El skill asociado a <paramref name="slot"/>. retorna null si <paramref name="slot"/> es <see cref="SkillSlot.None"/> o un valor incorrecto.</returns>
        public GenericSkill GetSkillBySkillSlot(SkillSlot slot)
        {
            switch (slot)
            {
                case SkillSlot.Primary: return primary;
                case SkillSlot.Secondary: return secondary;
                case SkillSlot.Special: return special;
            }
            return null;
        }

        /// <summary>
        /// Obtiene el <see cref="GenericSkill"/> de nombre <paramref name="name"/>
        /// </summary>
        /// <param name="name">El nombre del skill a conseguir</param>
        /// <returns>La skill con el nombre <paramref name="name"/>. Retorna null si no se encuentra ninguna skill con el nombre especificado.</returns>
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