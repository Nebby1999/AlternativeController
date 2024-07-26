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
        private Vehicle _lastConnectedVehicle;
        private void Awake()
        {
            hingeJoint2D = GetComponent<HingeJoint2D>();
            rigidbody2D = GetComponent<Rigidbody2D>();
            isConnected = false;
        }

        private void Start()
        {
            OnVehicleConnectedChange();
        }

        private void OnVehicleConnectedChange()
        {
            rigidbody2D.bodyType = isConnected ? RigidbodyType2D.Dynamic : RigidbodyType2D.Static;
            hingeJoint2D.enabled = isConnected;
            hingeJoint2D.connectedBody = isConnected ? connectedVehicle.GetComponent<Rigidbody2D>() : null;

            if (isConnected)
                connectedVehicle.connectedCargo = this;
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

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (isConnected)
                return;

            var t = collision.transform;

            var dotProduct = Vector3.Dot(t.up, transform.up);
            if (dotProduct < 0.75)
                return;

            if(collision.TryGetComponent<Vehicle>(out var vehicle) && !vehicle.isInCombatMode)
            {
                connectedVehicle = vehicle;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if(collision.TryGetComponent<Vehicle>(out var vehicle))
            {
                if(!connectedVehicle && vehicle == _lastConnectedVehicle)
                {
                    _lastConnectedVehicle = null;
                }
            }
        }
    }
}