using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AC
{
    /// <summary>
    /// Un Cargo es un objeto de los <see cref="Vehicle"/> el cual puede llevar <see cref="ResourceDef"/> minados por el vehiculo
    /// </summary>
    [RequireComponent(typeof(HingeJoint2D))]
    public class Cargo : MonoBehaviour
    {
        /// <summary>
        /// El HingeJoint del Cargo, esto es usado para darle fisica al vehiculo y al Cargo como si fuera un Camion de Carga.
        /// </summary>
        public HingeJoint2D hingeJoint2D { get; private set; }

        /// <summary>
        /// El rigidbody que tiene el cargo.
        /// </summary>
        public new Rigidbody2D rigidbody2D { get; private set; }

        /// <summary>
        /// El Vehiculo que esta actualmente conectado a este Cargo
        /// </summary>
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

        /// <summary>
        /// Retorna True si el cargo esta conectado a un vehiculo
        /// </summary>
        public bool isConnected { get; private set; }

        /// <summary>
        /// Retorna la capacidad maxima de recursos
        /// </summary>
        public int maxCapacity => _maxCapacity;
        [Tooltip("cuantos recursos puede llevar el cargo")]
        [SerializeField] private int _maxCapacity;

        [Tooltip("El prefab de un Resource Chunk, el cual es un objeto botado por el Cargo para alimentar al HQ o distraer enemigos")]
        [SerializeField] private ResourceChunk _chunkPrefab;
        private int[] _mineralCount;

        /// <summary>
        /// Una Queue que tiene el orden de recursos coleccionados usando sus <see cref="ResourceIndex"/>
        /// </summary>
        public Queue<ResourceIndex> resourceCollectionOrder { get; private set; }

        /// <summary>
        /// El ultimo recurso que fue descargado del Cargo
        /// </summary>
        public ResourceIndex lastUnloadedResource { get; private set; }

        /// <summary>
        /// Retorna la cantidad total de objetos cargados en el Cargo
        /// </summary>
        public int totalCargoHeld => resourceCollectionOrder.Count;

        /// <summary>
        /// Retorna true si el Cargo esta lleno
        /// </summary>
        public bool isFull => totalCargoHeld >= _maxCapacity;

        /// <summary>
        /// Retorna true si el cargo esta vacio
        /// </summary>
        public bool isEmpty => totalCargoHeld <= 0;

        private Vehicle _lastConnectedVehicle;
        private Transform _transform;

        /// <summary>
        /// Suelta un <see cref="ResourceChunk"/> usando el prefab en <see cref="_chunkPrefab"/>.
        /// </summary>
        /// <param name="amount">La cantidad de Chunks a soltar</param>
        /// <returns>True si se soltaron chunks</returns>
        public bool DropResource(int amount)
        {
            if (isEmpty)
                return false;

            for(int i = 0; i < amount; i++)
            {
                lastUnloadedResource = resourceCollectionOrder.Dequeue();
                _mineralCount[(int)lastUnloadedResource]--;
                var instance = Instantiate(_chunkPrefab, _transform.position, _transform.rotation);
                instance.resourceDef = ResourceCatalog.GetResourceDef(lastUnloadedResource);
                instance.resourceValue = 1;
                instance.rigidbody2D.velocity = -(_transform.up) * 15;
            }
            return true;
        }

        /// <summary>
        /// Carga <paramref name="amount"/> cantidad de recursos de tipo <paramref name="resourceDef"/>.
        /// </summary>
        /// <param name="resource">El tipo de recurso</param>
        /// <param name="amount">La cantidad de recurso a Cargar</param>
        /// <returns>Verdadero si el proceso de carga funciono, si no, retorna falso</returns>
        public bool LoadResource(ResourceDef resource, int amount) => LoadResource(resource.resourceIndex, amount);

        /// <summary>
        /// Carga <paramref name="amount"/> cantidad de recursos de tipo <paramref name="resourceIndex"/>.
        /// </summary>
        /// <param name="index">El tipo de recurso</param>
        /// <param name="amount">La cantidad de recurso a Cargar</param>
        /// <returns>Verdadero si el proceso de carga funciono, si no, retorna falso</returns>
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

        /// <summary>
        /// Descarga <paramref name="amount"/> cantidad de recursos guardado en el cargo.
        /// </summary>
        /// <param name="amount">La cantidad de recurso a Descargar</param>
        /// <returns>Verdadero si el proceso de descarga funciono, si no, retorna falso</returns>
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

        /// <summary>
        /// Retorna la cantidad de recursos de tipo <paramref name="def"/> en el cargo.
        /// </summary>
        /// <param name="def">El tipo de recurso</param>
        /// <returns>La cantidad de recursos</returns>
        public int GetResourceCount(ResourceDef def) => def ? GetResourceCount(def.resourceIndex) : 0;

        /// <summary>
        /// Retorna la cantidad de recursos de indice <paramref name="index"/> en el cargo
        /// </summary>
        /// <param name="index">el indice de recurso</param>
        /// <returns>La cantidad de Recursos</returns>
        public int GetResourceCount(ResourceIndex index) => _mineralCount[(int)index];

        private void Awake()
        {
            _mineralCount = new int[ResourceCatalog.resourceCount];
            hingeJoint2D = GetComponent<HingeJoint2D>();
            rigidbody2D = GetComponent<Rigidbody2D>();
            isConnected = false;
            resourceCollectionOrder = new Queue<ResourceIndex>(_maxCapacity);
            _transform = transform;
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

        /// <summary>
        /// Desconecta este cargo del vehiculo
        /// </summary>
        public void DetachCargo()
        {
            connectedVehicle.connectedCargo = null;
            connectedVehicle = null;
        }

        private void OnJointBreak2D(Joint2D joint)
        {
            DetachCargo();
        }

        /// <summary>
        /// Metodo que es ejecutado cuando un collider esta en el radio de coneccion del vehiculo.
        /// 
        /// <br></br>
        /// Revisa 
        /// <see cref="OnTrigger2DEventBehaviour"/>
        /// </summary>
        /// <param name="collision">La colision en si</param>
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

        /// <summary>
        /// Metodo que es ejecutado cuando un collider sale del radio de coneccion del vehiculo.
        /// 
        /// <br></br>
        /// Revisa 
        /// <see cref="OnTrigger2DEventBehaviour"/>
        /// </summary>
        /// <param name="collision">La colision en si</param>
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