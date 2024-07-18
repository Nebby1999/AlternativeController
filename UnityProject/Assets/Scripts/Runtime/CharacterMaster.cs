using Nebula;
using UnityEngine;

namespace AC
{
    public class CharacterMaster : MonoBehaviour
    {
        [SerializeField, ForcePrefab] private CharacterBody _defaultBodyPrefab;
        public bool spawnOnStart;

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

        public CharacterBody bodyInstance { get; private set; }
        
        public InputBank bodyInputBank { get; private set; }

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
            }
        }
    }

    public interface ICharacterInputProvider
    {
        Vector2 movementVector { get; }

        int rotationInput { get; }
    
        bool primaryInput { get; }

        bool secondaryInput { get; }
    }
}