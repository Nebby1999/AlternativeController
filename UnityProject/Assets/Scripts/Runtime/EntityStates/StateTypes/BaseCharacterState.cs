using UnityEngine;

namespace EntityStates
{
    public class BaseCharacterState : EntityState
    {
        public bool hasCharacterBody { get; private set; }
        public bool hasSkillManager { get; private set; }
        public bool hasInputBank { get; private set; }
        public float attackSpeedStat;
        public float movementSpeedStat;
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

        protected Ray GetAimRay()
        {
            return new Ray(transform.position, transform.up);
        }
    }
}