using UnityEngine;
using AC;

namespace States
{

    public class Harvest : State<Vehicle_OLD.State, Vehicle_OLD>
    {
        public Harvest(Vehicle_OLD context) : base(Vehicle_OLD.State.Harvest, context) {}
        public override void EnterState()
        {
            Context.OnStateChanged();
            Debug.Log("HARVEST");
        }

        public override void ExitState()
        {
            Context.ThrowCargo();
        }

        public override Vehicle_OLD.State GetNextState()
        {
            return Context.IsBattle ? Vehicle_OLD.State.Combat : Vehicle_OLD.State.Harvest;
        }

        public override void OnTriggerEnter(Collider other)
        {
        }

        public override void OnTriggerExit(Collider other)
        {
        }

        public override void OnTriggerStay(Collider other)
        {
            if(other.TryGetComponent<IHarvesteable>(out IHarvesteable loadeable)) Context.Harvest(loadeable);
            if(other.TryGetComponent<HeadQuarters_OLD>(out HeadQuarters_OLD hq)) Context.Deliver(hq);
        }

        public override void UpdateState()
        {
            Context.Decoy();
        }
    }
}