using Nebula;
using System.Collections.Generic;

namespace AC
{
    public class BugTargetSelector : IAITargetSelector
    {
        private List<Base> _bases;
        private List<ResourceOreDeposit> _resourceOreDeposit;
        private List<PlayableCharacterMaster> _players;

        public void Initialize(object sender)
        {
            _bases = InstanceTracker.GetInstances<Base>();
            _resourceOreDeposit = InstanceTracker.GetInstances<ResourceOreDeposit>();
            _players = InstanceTracker.GetInstances<PlayableCharacterMaster>();
        }

        public BaseAI.Target GetTarget(BaseAI baseAI, AIDriver driver)
        {
            return null;
        }
    }
}