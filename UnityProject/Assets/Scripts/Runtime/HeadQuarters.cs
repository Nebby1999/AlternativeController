using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace AC
{
    //Could be a singleton?
    [RequireComponent(typeof(ResourcesManager))]
    [RequireComponent(typeof(HeadQuartersInputProvider))]
    public class HeadQuarters : MonoBehaviour
    {
        private const int TOTAL_VEHICLE_COUNT = 1;
        [SerializeField] private bool _spawnVehicleMasterOnStart;
        [SerializeField] private PlayableCharacterMaster _vehicleMasterPrefab;
        [SerializeField] private Transform _vehicleSpawnPosition;
        public Base[] bases { get; private set; }
        [SerializeField] private Base _redBase;
        [SerializeField] private Base _blackBase;
        [SerializeField] private float _timeBetweenResourceLoads;

        public HeadQuartersInputProvider inputProvider { get; private set; }
        public HeadQuartersInputProvider.HQInput inputStruct { get; private set; }
        public ResourcesManager resourcesManager { get; private set; }

        private HashSet<Cargo> _cargosOnTrigger = new HashSet<Cargo>();
        private CharacterMaster[] _vehicleMasters = new CharacterMaster[TOTAL_VEHICLE_COUNT];
        private Vehicle[] _cachedVehicleComponents = new Vehicle[TOTAL_VEHICLE_COUNT];
        private ResourceIndex _blackIndex;
        private float _resourceObtainStopwatch;
        private CircleSearch _chunkSearch = new CircleSearch();
        private void Awake()
        {
            resourcesManager = GetComponent<ResourcesManager>();
            inputProvider = GetComponent<HeadQuartersInputProvider>();
            bases = new Base[2]
            {
                _blackBase,
                _redBase
            };
            _chunkSearch = new CircleSearch
            {
                candidateMask = LayerIndex.pickups.mask,
                origin = transform.position,
                radius = 1 * transform.localScale.magnitude,
                searcher = gameObject,
                useTriggers = false,
            };
        }

        private void Start()
        {
            _blackIndex = ResourceCatalog.FindResource("Black");

            if(_spawnVehicleMasterOnStart)
            {
                SpawnVehicleMasters();
            }
        }

        private void SpawnVehicleMasters()
        {
            for (int i = 0; i < TOTAL_VEHICLE_COUNT; i++)
            {
                Vector3 offset = _vehicleSpawnPosition.position;
                offset.x += i == 0 ? -1.5f : 1.5f;

                _vehicleMasters[i] = Instantiate(_vehicleMasterPrefab, offset, _vehicleSpawnPosition.rotation).GetComponent<CharacterMaster>();
            }
        }

        private void Update()
        {
            inputStruct = inputProvider.GetInputs();            
        }

        private void FixedUpdate()
        {
            for (int i = 0; i < TOTAL_VEHICLE_COUNT; i++)
            {
                var vehicleMaster = _vehicleMasters[i];
                Vehicle vehicleComponent = _cachedVehicleComponents[i];
                if (!vehicleComponent)
                {
                    vehicleComponent = vehicleMaster.bodyInstance ? vehicleMaster.bodyInstance.GetComponent<Vehicle>() : null;
                    _cachedVehicleComponents[i] = vehicleComponent;
                }

                if (!vehicleComponent)
                    continue;

                switch(i)
                {
                    case 0:
                        vehicleComponent.isInCombatMode = inputStruct.vehicle1CombatMode; break;
                    case 1:
                        vehicleComponent.isInCombatMode = inputStruct.vehicle2CombatMode; break;
                }
            }

            _resourceObtainStopwatch += Time.fixedDeltaTime;
            if(_resourceObtainStopwatch > _timeBetweenResourceLoads)
            {
                _resourceObtainStopwatch -= _timeBetweenResourceLoads;
                TryUnloadCargos();
                TryLoadChunks();
            }
            foreach(var resourceIndex in resourcesManager.resourceTypesStored)
            {
                TrySupplyBases(resourceIndex);
            }
        }

        private void TryUnloadCargos()
        {
            foreach(var cargo in _cargosOnTrigger)
            {
                if (cargo.isConnected)
                    continue;

                if (cargo.UnloadResource(1))
                {
                    TryLoadResource(cargo.lastUnloadedResource, 1);
                }
            }
        }

        private void TryLoadChunks()
        {
            _chunkSearch.FindCandidates()
                .FilterCandidatesByComponent<ResourceChunk>()
                .GetResults(out var results);

            foreach(var result in results)
            {
                ResourceChunk chunk = (ResourceChunk)result.componentChosenDuringFilterByComponent;
                if(TryLoadResource(chunk.resourceDef, chunk.resourceValue))
                {
                    Destroy(chunk.gameObject);
                }
            }
        }

        private void TrySupplyBases(ResourceIndex index)
        {
            for(int i = 0; i < bases.Length; i++)
            {
                bool input = false;
                if(index == _blackIndex)
                {
                    if(i == 0)
                    {
                        input = inputStruct.blackResourceDestination == -1;
                    }
                    else
                    {
                        input = inputStruct.blackResourceDestination == 1;
                    }
                }
                else
                {
                    if (i == 1)
                    {
                        input = inputStruct.redResourceDestination == -1;
                    }
                    else
                    {
                        input = inputStruct.redResourceDestination == 1;
                    }
                }

                if (input && resourcesManager.UnloadResource(index, 0.04f))
                {
                    bases[i].TryLoadMineral(index, 0.04f);
                }
            }
        }

        public bool TryLoadResource(ResourceDef resource, float amount) => TryLoadResource(resource ? resource.resourceIndex : ResourceIndex.None, amount);

        public bool TryLoadResource(ResourceIndex index, float amount)
        {
            return resourcesManager.LoadResource(index, amount);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.TryGetComponent<Cargo>(out var cargo))
            {
                _cargosOnTrigger.Add(cargo);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.TryGetComponent<Cargo>(out var cargo))
            {
                _cargosOnTrigger.Remove(cargo);
            }
        }
    }
}