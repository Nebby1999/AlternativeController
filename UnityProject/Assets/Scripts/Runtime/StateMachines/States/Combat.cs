using UnityEngine;
using AC;

namespace States
{

    public class Combat : State<Vehicle_OLD.State, Vehicle_OLD>
    {
        public Combat(Vehicle_OLD context) : base(Vehicle_OLD.State.Combat, context) {}
        public override void EnterState()
        {
            Context.OnStateChanged();
            Debug.Log("COMBAT");
        }

        public override void ExitState()
        {
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
        }

        public override void UpdateState()
        {        
            Context.Attack();
            Context.Defense();
            Context.Stun();
        }
    }
}