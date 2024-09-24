using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace AC
{
    [RequireComponent(typeof(CharacterMaster))]
    [RequireComponent(typeof(NavMeshAgent))]
    public class BaseAI : MonoBehaviour, ICharacterInputProvider
    {
        public CharacterMaster characterMaster { get; private set; }
        public NavMeshAgent navMeshAgent { get; private set; }

        public Vector2 movementVector { get; private set; }

        public int rotationInput { get; private set; }

        public bool primaryInput { get; private set; }

        public bool secondaryInput { get; private set; }

        public bool specialInput { get; private set; }

        public Transform target;
        private void Awake()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            characterMaster = GetComponent<CharacterMaster>();
        }

        // Start is called before the first frame update
        void Start()
        {
            navMeshAgent.updatePosition = false;
            navMeshAgent.updateRotation = false;
            navMeshAgent.updateUpAxis = false;
        }

        private void Update()
        {
            navMeshAgent.nextPosition = characterMaster.bodyInstance.transform.position;
            navMeshAgent.speed = characterMaster.bodyInstance.movementSpeed;

            if (!navMeshAgent.hasPath)
                return;

            if (!characterMaster.bodyInstance) 
                return;

            movementVector = navMeshAgent.desiredVelocity.normalized;
        }

        [ContextMenu("Create Path to Transform")]
        private void ContextMenu()
        {
            var path = new NavMeshPath();
            navMeshAgent.CalculatePath(target.position, path);
            navMeshAgent.SetPath(path);
        }
    }
}
