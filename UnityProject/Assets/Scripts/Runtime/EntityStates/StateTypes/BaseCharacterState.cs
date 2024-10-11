using UnityEngine;

namespace EntityStates
{
    /// <summary>
    /// Estado base usado por maquinas que controlan un <see cref="AC.CharacterBody"/>
    /// </summary>
    public class BaseCharacterState : EntityState
    {
        /// <summary>
        /// Verdadero si la maquina tiene un CharacterBody
        /// </summary>
        public bool hasCharacterBody { get; private set; }

        /// <summary>
        /// Verdadero si la maquina tiene un SkillManager
        /// </summary>
        public bool hasSkillManager { get; private set; }

        /// <summary>
        /// Verdadero si la maquina tiene un Input Bank
        /// </summary>
        public bool hasInputBank { get; private set; }

        /// <summary>
        /// El stat de velocidad de ataque del cuerpo relacionado a esta maquina
        /// </summary>
        public float attackSpeedStat;

        /// <summary>
        /// El stat de velocidad de movimiento del cuerpo relacionado a esta maquina
        /// </summary>
        public float movementSpeedStat;

        /// <summary>
        /// El stat daño del cuerpo relacionado a esta maquina
        /// </summary>
        public float damageStat;

        public override void OnEnter()
        {
            base.OnEnter();
            hasCharacterBody = characterBody;
            if(hasCharacterBody)
            {
                attackSpeedStat = characterBody.attackSpeed;
                movementSpeedStat = characterBody.movementSpeed;
                damageStat = characterBody.damage;
            }
            hasSkillManager = skillManager;
            hasInputBank = inputBank;
        }

        /// <summary>
        /// Devuelve un <see cref="Ray"/>, el cual muestra en que direccion el cuerpo esta mirando.
        /// </summary>
        protected Ray GetAimRay()
        {
            return new Ray(transform.position, transform.up);
        }
    }
}