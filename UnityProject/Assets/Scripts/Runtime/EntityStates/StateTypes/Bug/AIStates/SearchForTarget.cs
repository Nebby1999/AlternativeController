using AC;
using Nebula;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EntityStates.Bug.AI
{
    public class SearchForTarget : BaseAIState
    {
        private List<PlayableCharacterMaster> _playerInstances;
        private List<Base> _baseInstances;
        private List<ResourceOreDeposit> _oreDepositInstances;

        public override void OnEnter()
        {
            base.OnEnter();
            _playerInstances = InstanceTracker.GetInstances<PlayableCharacterMaster>();
            _baseInstances = InstanceTracker.GetInstances<Base>();
            _oreDepositInstances = InstanceTracker.GetInstances<ResourceOreDeposit>();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
        }
    }
}
