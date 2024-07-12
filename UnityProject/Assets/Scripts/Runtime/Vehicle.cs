using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using States;

namespace AC
{

    [RequireComponent(typeof(MacroKeyboard))]
    [RequireComponent(typeof(Movement))]
    [RequireComponent(typeof(VisualController))]
    [RequireComponent(typeof(ShootController))]
    [RequireComponent(typeof(StunController))]
    public class Vehicle : StateManager<Vehicle.State, Vehicle>
    {
        [HideInInspector] public enum State
        {
            Harvest,
            Combat
        }
        private StunController _stunController;
        private DecoyController _decoyController;
        private ShootController _shootController;
        private Shield _shieldController;
        private VisualController _visualController;
        private Movement _movementController;
        private MacroKeyboard _input;
        [SerializeField] private CommandBuffer _vehicleBuffer;
        [SerializeField] private Cargo _cargo;
        [SerializeField] private GameObjectPool _decoyPool;
        public bool IsBattle;
        protected override void InitializeStates()
        {
            Debug.Log("Initializing States");
            State<State, Vehicle> harvest = new Harvest(this);
            State<State, Vehicle> combat = new Combat(this);
            States.Add(State.Harvest, harvest);
            States.Add(State.Combat, combat);
            CurrentState = States[State.Harvest];
        }
        protected override void Awake()
        {
            base.Awake();
            _movementController = GetComponent<Movement>();
            _input = GetComponent<MacroKeyboard>();
            _visualController = GetComponent<VisualController>();
            _shootController = GetComponent<ShootController>();
            _shieldController = GetComponent<Shield>();
            _stunController = GetComponent<StunController>();
            _cargo = new Cargo(10);
            _decoyController = new DecoyController(_decoyPool, transform);
        }
        protected override void Update()
        {
            base.Update();
            _vehicleBuffer.ExecuteQueue();
            _visualController.VisualFeedback(_input.GetActionDown(2));
            _movementController.TryMovement(_input.GetActionDown(2));
            _movementController.TryRotate(_input.GetRotation());
        }
        public void ThrowCargo()
        {
            foreach(ResourceIndex mineral in _cargo.resourceCollectionOrder.ToList())
                _decoyController.TryDecoy(_cargo.UnloadResource(1), _cargo.lastUnloadedResource);
        }
        public void Harvest(IHarvesteable sender)
        {
            if(_input.GetAction(1)) _vehicleBuffer.QueueCommand(new Collect(_cargo,_input,sender,1));
        }
        public void Deliver(HeadQuarters hq)
        {
            if(_input.GetAction(0)) _vehicleBuffer.QueueCommand(new Deliver(_cargo, _input, hq, 1));
        }
        public void Decoy()
        {
            _decoyController.TryDecoy(_input.GetActionDown(5) && _cargo.UnloadResource(1), _cargo.lastUnloadedResource); 
        }
        public void Attack()
        {
            _shootController.TryShooting(_input.GetActionDown(0));
        }
        public void Defense()
        {
            _movementController.SetMoveBool(_input.GetAction(1));
            _shieldController.TryDefense(_input.GetAction(1));
        }
        public void Stun()
        {
            _stunController.TryStun(_input.GetActionDown(5));
        }
        public void OnStateChanged()
        {
            bool isHarvest = CurrentState ==  States[State.Harvest];
            float movement = isHarvest ? 1f : 1.25f;
            float rotation = isHarvest ? 1000f : 1250f;
            Material material = GetComponent<Renderer>().material;
            material.color = isHarvest ? Color.green : Color.blue;
            _movementController.ChangeValues(movement, rotation);
            _shieldController.enabled = !isHarvest;
            _shootController.enabled = !isHarvest;
            _stunController.enabled = !isHarvest;
        }
    }
}
