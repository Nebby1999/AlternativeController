using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AC
{
    [RequireComponent(typeof(HingeJoint2D))]
    public class Cargo : MonoBehaviour
    {
        public HingeJoint2D hingeJoint2D { get; private set; }
        public new Rigidbody2D rigidbody2D { get; private set; }
        public Vehicle connectedVehicle
        {
            get => _connectedVehicle;
            private set
            {
                if(_connectedVehicle != value)
                {
                    _connectedVehicle = value;
                    isConnected = value;
                    OnVehicleConnectedChange();
                }
            }
        }
        private Vehicle _connectedVehicle;

        public bool isConnected { get; private set; }

        public int maxCapacity => _maxCapacity;
        [SerializeField] private int _maxCapacity;
        private int[] _mineralCount;

        public Queue<ResourceIndex> resourceCollectionOrder { get; private set; }
        public ResourceIndex lastUnloadedResource { get; private set; }
        public int totalCargoHeld => resourceCollectionOrder.Count;
        public bool isFull => totalCargoHeld >= _maxCapacity;
        public bool isEmpty => totalCargoHeld <= 0;

        private Vehicle _lastConnectedVehicle;
        public bool LoadResource(ResourceDef resource, int amount) => LoadResource(resource.resourceIndex, amount);

        public bool LoadResource(ResourceIndex index, int amount)
        {
            if (index == ResourceIndex.None || isFull)
                return false;

            var indexAsInt = (int)index;
            _mineralCount[indexAsInt] += amount;
            for (int i = 0; i < amount; i++)
            {
                resourceCollectionOrder.Enqueue(index);
            }
            return true;
        }


        public bool UnloadResource(int amount)
        {
            if (isEmpty)
                return false;

            for (int i = 0; i < amount; i++)
            {
                lastUnloadedResource = resourceCollectionOrder.Dequeue();
                _mineralCount[(int)lastUnloadedResource]--;
            }
            return true;
        }

        public int GetResourceCount(ResourceDef def) => def ? GetResourceCount(def.resourceIndex) : 0;
        public int GetResourceCount(ResourceIndex index) => _mineralCount[(int)index];

        private void Awake()
        {
            _mineralCount = new int[ResourceCatalog.resourceCount];
            hingeJoint2D = GetComponent<HingeJoint2D>();
            rigidbody2D = GetComponent<Rigidbody2D>();
            isConnected = false;
            resourceCollectionOrder = new Queue<ResourceIndex>(_maxCapacity);
        }

        private void Start()
        {
            OnVehicleConnectedChange();
        }

        private void OnVehicleConnectedChange()
        {
            rigidbody2D.bodyType = isConnected ? RigidbodyType2D.Dynamic : RigidbodyType2D.Kinematic;
            hingeJoint2D.enabled = isConnected;
            hingeJoint2D.connectedBody = isConnected ? connectedVehicle.GetComponent<Rigidbody2D>() : null;

            if (isConnected)
                connectedVehicle.connectedCargo = this;
            else
            {
                rigidbody2D.velocity = Vector2.zero; 
                rigidbody2D.angularVelocity = 0;
            }
        }

        public void DetachCargo()
        {
            connectedVehicle.connectedCargo = null;
            connectedVehicle = null;
        }

        private void OnJointBreak2D(Joint2D joint)
        {
            DetachCargo();
        }

        public void OnConnectionRadiusStay(Collider2D collision)
        {
            if (isConnected)
                return;

            var t = collision.transform;

            var dotProduct = Vector3.Dot(t.up, transform.up);
            if (dotProduct < 0.75)
                return;

            if (collision.TryGetComponent<Vehicle>(out var vehicle) && !vehicle.isInCombatMode)
            {
                connectedVehicle = vehicle;
            }
        }

        public void OnConnectionRadiusExit(Collider2D collision)
        {
            if (collision.TryGetComponent<Vehicle>(out var vehicle))
            {
                if (!connectedVehicle && vehicle == _lastConnectedVehicle)
                {
                    _lastConnectedVehicle = null;
                }
            }
        }
    }
}