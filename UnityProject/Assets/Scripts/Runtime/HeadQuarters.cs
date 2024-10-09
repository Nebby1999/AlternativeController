using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace AC
{
    /// <summary>
    /// Representa los Headquarters de los jugadores, uno de los tres jugadores maneja el HQ. y su mision es tener que controlar los recursos de las bases y los estados de los jugadores.
    /// </summary>
    //TODO: Pudiera ser un singleton?
    [RequireComponent(typeof(ResourcesManager))]
    [RequireComponent(typeof(HeadQuartersInputProvider))]
    public class HeadQuarters : MonoBehaviour
    {
        //La cantidad total de vehiculos.
        private const int TOTAL_VEHICLE_COUNT = 1;
        [Tooltip("Los vehiculos de los jugadores deberian aparecer en Start?")]
        [SerializeField] private bool _spawnVehicleMasterOnStart;
        [Tooltip("El prefab maestro de los vehiculos")]
        [SerializeField] private PlayableCharacterMaster _vehicleMasterPrefab;
        [Tooltip("El transform donde los vehiculos aparecen.")]
        [SerializeField] private Transform _vehicleSpawnPosition;

        /// <summary>
        /// Las bases manejadas por el Headquarters
        /// </summary>
        public Base[] bases { get; private set; }
        [Tooltip("La base roja")]
        [SerializeField] private Base _redBase;

        [Tooltip("La base negra")]
        [SerializeField] private Base _blackBase;

        [Tooltip("El tiempo entre intentos de descarga de recursos.")]
        [SerializeField] private float _timeBetweenResourceLoads;

        /// <summary>
        /// El <see cref="HeadQuartersInputProvider"/> de headquarters
        /// 
        /// </summary>
        public HeadQuartersInputProvider inputProvider { get; private set; }

        /// <summary>
        /// El input actual de headquarters
        /// </summary>
        public HeadQuartersInputProvider.HQInput inputStruct { get; private set; }

        /// <summary>
        /// El <see cref="ResourcesManager"/> del HQ
        /// </summary>
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
                if (!vehicleComponent) //Consigue el vehiculo actual del jugador
                {
                    vehicleComponent = vehicleMaster.bodyInstance ? vehicleMaster.bodyInstance.GetComponent<Vehicle>() : null;
                    _cachedVehicleComponents[i] = vehicleComponent;
                }

                if (!vehicleComponent)
                    continue;

                //Cambiar modo de combate del vehiculo
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
            //Descarga cualquier cargo que no esta conectado y se encuentra en el trigger
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
            //Ocupa el ChunkSearch para encontrar los ResourceChunks cercanos
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
                    bases[i].TryLoadResource(index, 0.04f);
                }
            }
        }

        /// <summary>
        /// <inheritdoc cref="ResourcesManager.LoadResource(ResourceDef, float)"/>
        /// </summary>
        public bool TryLoadResource(ResourceDef resource, float amount) => TryLoadResource(resource ? resource.resourceIndex : ResourceIndex.None, amount);

        /// <summary>
        /// <inheritdoc cref="ResourcesManager.LoadResource(ResourceIndex, float)"/>
        /// </summary>
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