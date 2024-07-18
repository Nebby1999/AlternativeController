using UnityEngine;

namespace AC
{
    //Could be a singleton?
    [RequireComponent(typeof(ResourcesManager))]
    public class HeadQuarters : MonoBehaviour
    {
        [SerializeField] private bool _spawnVehicleMasterOnStart;
        [SerializeField] private PlayableCharacterMaster _vehicleMasterPrefab;
        [SerializeField] private Transform _vehicleSpawnPosition;

        public ResourcesManager resourcesManager { get; private set; }

        private Vehicle _vehicle;
        private void Awake()
        {
            resourcesManager = GetComponent<ResourcesManager>();
        }

        private void Start()
        {
            if(_spawnVehicleMasterOnStart)
            {
                SpawnVehicleMasters();
            }
        }

        private void SpawnVehicleMasters()
        {
            var instance = Instantiate(_vehicleMasterPrefab, _vehicleSpawnPosition.position, _vehicleSpawnPosition.rotation);
            _vehicle = instance.GetComponent<Vehicle>();
        }

        public void TryLoadResource(ResourceDef resource, int amount) => TryLoadResource(resource ? resource.resourceIndex : ResourceIndex.None, amount);

        public void TryLoadResource(ResourceIndex index, int amount)
        {
            resourcesManager.LoadResource(index, amount);
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if(collision.TryGetComponent<Vehicle>(out var vehicle))
            {
            }
        }
    }
}