using UnityEngine;
using AC;

namespace States
{

    public class Harvest : State<Vehicle.State, Vehicle>
    {
        public Harvest(Vehicle context) : base(Vehicle.State.Harvest, context) {}
        public override void EnterState()
        {
            Context.OnStateChanged();
            Debug.Log("HARVEST");
        }

        public override void ExitState()
        {
            Context.ThrowCargo();
        }

        public override Vehicle.State GetNextState()
        {
            return Context.IsBattle ? Vehicle.State.Combat : Vehicle.State.Harvest;
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
            if(other.TryGetComponent<HeadQuarters>(out HeadQuarters hq)) Context.Deliver(hq);
        }

        public override void UpdateState()
        {
            Context.Decoy();
        }
    }
}