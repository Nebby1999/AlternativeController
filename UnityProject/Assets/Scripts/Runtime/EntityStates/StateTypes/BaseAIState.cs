using AC;
using Nebula;
using System;

namespace EntityStates
{
    public class BaseAIState : EntityState
    {
        public new SkillManager skillManager => throw new NotSupportedException("BaseAI State has no support for skill managers.");
        public new Rigidbody2DCharacterController characterController => throw new NotSupportedException("BaseAIState has no support for Rigidbody2D character controllers.");
        public new InputBank inputBank => throw new NotSupportedException("Do not interact with the input bank directly, override \"ProvideInputs\" instead.");
        public new HealthComponent healthComponent => characterMaster.bodyInstance ? characterMaster.bodyInstance.healthComponent : null;
        public new CharacterBody characterBody => characterMaster.bodyInstance;
        public bool hasBody => characterMaster.bodyInstance;
        public BaseAI baseAI { get; private set; }
        public Xoroshiro128Plus aiRNG => baseAI.aiRNG;
        public CharacterMaster characterMaster { get; private set; }
        public ResourceDef preferredResourceDef { get; private set; }

        public override void OnEnter()
        {
            base.OnEnter();
            baseAI = GetComponent<BaseAI>();
            characterMaster = GetComponent<CharacterMaster>();
            if(baseAI.resourceDefPreference)
            {
                preferredResourceDef = baseAI.resourceDefPreference.resourcePreference;
            }
        }
    }
}