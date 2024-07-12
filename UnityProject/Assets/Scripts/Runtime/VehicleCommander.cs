using UnityEngine;
using UnityEngine.InputSystem;

namespace AC
{
    [RequireComponent(typeof(PlayerInput))]
    public class VehicleCommander : MonoBehaviour
    {
        private GameObject vehiclePrefab;
        public PlayerInput playerInput { get; private set; }

        private void Awake()
        {
            playerInput = GetComponent<PlayerInput>();
        }
    }
}