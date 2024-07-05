using UnityEngine;
using AC;

namespace States
{

    public class Combat : State<Vehicle.State, Vehicle>
    {
        public Combat(Vehicle context) : base(Vehicle.State.Combat, context) {}
        public override void EnterState()
        {
            Context.OnStateChanged();
            Debug.Log("COMBAT");
        }

        public override void ExitState()
        {
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
        }

        public override void UpdateState()
        {        
            Context.Attack();
            Context.Defense();
            Context.Stun();
        }
    }
}