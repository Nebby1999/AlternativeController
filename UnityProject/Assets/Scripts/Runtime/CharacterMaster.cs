using Nebula;
using UnityEngine;

namespace AC
{
    /// <summary>
    /// Un <see cref="CharacterMaster"/> es un GameObject el cual representa el "Cerebro" de un Personaje.
    /// <para></para>
    /// El motivo de un CharacterMaster es ser un objeto el cual controla las acciones de un GameObject el cual es, en si, el personaje dentro de la escena. El CharacterMaster no es nada mas que un Cerebro que controla un cuerpo en la escena.
    /// <br></br>
    /// Al estar el cuerpo y el "cerebro" desacoplados, un developer puede generar el cuerpo de un enemigo, y probarlo de una manera directa y controlarlo. cambiando el valor de <see cref="_defaultBodyPrefab"/> dentro de unity. o <see cref="bodyPrefab"/> dentro de codigo.
    /// </summary>
    public class CharacterMaster : MonoBehaviour
    {
        [Tooltip("El prefab de cuerpo para este maestro")]
        [SerializeField, ForcePrefab] private CharacterBody _defaultBodyPrefab;
        [Tooltip("Si el maestro deberia hacer aparecer su cuerpo en Start")]
        public bool spawnOnStart;

        /// <summary>
        /// El Prefab actual de Cuerpo para este maestro
        /// </summary>
        public GameObject bodyPrefab
        {
            get
            {
                if(!_bodyPrefab)
                {
                    _bodyPrefab = _defaultBodyPrefab.gameObject;
                }
                return _bodyPrefab;
            }
            set
            {
                if(value.TryGetComponent<CharacterBody>(out _))
                {
                    _bodyPrefab = value;
                    return;
                }
                Debug.LogWarning($"Supplied GameObject is not a CharacterBody prefab.", value);
            }
        }
        private GameObject _bodyPrefab;

        /// <summary>
        /// El cuerpo actual de este maestro, esta es la instancia y por ende se considera un cuerpo "Vivo"
        /// </summary>
        public CharacterBody bodyInstance { get; private set; }
        
        /// <summary>
        /// El <see cref="InputBank"/> relacionado al Cuerpo. El maestro da los inputs al InputBank causando que el cuerpo se mueva dentro de la escena
        /// </summary>
        public InputBank bodyInputBank { get; private set; }

        /// <summary>
        /// El provider de inputs actual para este maestro
        /// </summary>
        public ICharacterInputProvider characterInputProvider { get; private set; }

        private void Awake()
        {
            characterInputProvider = GetComponent<ICharacterInputProvider>();
        }

        private void Start()
        {
            if(spawnOnStart)
            {
                SpawnHere();
            }
        }

        private void SpawnHere() => Spawn(transform.position, Quaternion.identity, false);

        private void Spawn(Vector3 position, Quaternion rotation, bool respawn)
        {
            if(!respawn && bodyInstance)
            {
                return;
            }

            if(bodyInstance)
            {
                Destroy(bodyInstance.gameObject);
                bodyInstance = null;
            }

            var newBody = Instantiate(bodyPrefab, position, rotation);
            bodyInstance = newBody.GetComponent<CharacterBody>();
            if(bodyInstance)
            {
                bodyInputBank = bodyInstance.inputBank;
                if(bodyInstance.TryGetComponent<ResourceDefPreference>(out var dest) && TryGetComponent<ResourceDefPreference>(out var src))
                {
                    dest.resourcePreference = src.resourcePreference;
                }
            }
        }

        private void Update()
        {
            if (bodyInputBank && characterInputProvider != null)
            {
                bodyInputBank.movementInput = characterInputProvider.movementVector;
                bodyInputBank.rotationInput = characterInputProvider.rotationInput;
                bodyInputBank.primaryButton.PushState(characterInputProvider.primaryInput);
                bodyInputBank.secondaryButton.PushState(characterInputProvider.secondaryInput);
                bodyInputBank.specialButton.PushState(characterInputProvider.specialInput);
            }
        }
    }

    /// <summary>
    /// Interfaz que representa los inputs de un CharacterMaster.
    /// </summary>
    public interface ICharacterInputProvider
    {
        /// <summary>
        /// El vector de movimiento deseado para el cuerpo
        /// </summary>
        Vector2 movementVector { get; }

        /// <summary>
        /// El input de rotacion deseado para el cuerpo.
        /// </summary>
        int rotationInput { get; }

        /// <summary>
        /// El input para la habilidad primaria
        /// </summary>
        bool primaryInput { get; }

        /// <summary>
        /// El input para la habilidad segundaria.
        /// </summary>
        bool secondaryInput { get; }

        /// <summary>
        /// El input para la habilidad Especial.
        /// </summary>
        bool specialInput { get; }
    }
}