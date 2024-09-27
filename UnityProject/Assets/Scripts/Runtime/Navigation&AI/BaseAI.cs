using EntityStates;
using Nebula;
using Nebula.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace AC
{
    [RequireComponent(typeof(CharacterMaster))]
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(EntityStateMachine))]
    public class BaseAI : MonoBehaviour, ICharacterInputProvider
    {
        public CharacterMaster characterMaster { get; private set; }
        public NavMeshAgent navMeshAgent { get; private set; }

        public Vector2 movementVector { get; private set; }

        public int rotationInput { get; private set; }

        public bool primaryInput { get; private set; }

        public bool secondaryInput { get; private set; }

        public bool specialInput { get; private set; }

        public Target currentTarget
        {
            get => _currentTarget;
            set
            {
                if(_currentTarget != value)
                {
                    _currentTarget = value;
                    SetNewTargetPosition();
                }
            }
        }
        public Target _currentTarget;
        public EntityStateMachine aiStateMachine { get; private set; }
        public Xoroshiro128Plus aiRNG { get; private set; }

        public ResourceDefPreference? resourceDefPreference { get; private set; }

        [SerializableSystemType.RequiredBaseType(typeof(BaseAIState))]
        public SerializableSystemType searchState;
        public bool searchOnStart;
#if DEBUG
        [Nebula.DisabledField]
        public Vector3 currentTargetPosition;
        [Nebula.DisabledField]
        public GameObject currentTargetObject;
#endif

        private void Awake()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            characterMaster = GetComponent<CharacterMaster>();
            aiStateMachine = GetComponent<EntityStateMachine>();
            resourceDefPreference = GetComponent<ResourceDefPreference>();
        }

        // Start is called before the first frame update
        void Start()
        {
            navMeshAgent.updatePosition = false;
            navMeshAgent.updateRotation = false;
            navMeshAgent.updateUpAxis = false;
            aiRNG = new Xoroshiro128Plus(ACApplication.instance.applicationRNG.nextULong);
            if(searchOnStart)
            {
                aiStateMachine.SetNextState(EntityStateCatalog.InstantiateState(searchState));
            }
        }

        private void FixedUpdate()
        {
            if (currentTarget == null)
            {
                return;
            }

#if DEBUG
            currentTargetPosition = currentTarget?.targetPosition ?? new Vector3(float.NaN, float.NaN);
            currentTargetObject = currentTarget?.targetObject ?? null;
#endif
            navMeshAgent.nextPosition = characterMaster.bodyInstance.transform.position;
            navMeshAgent.speed = characterMaster.bodyInstance.movementSpeed;

            if (!navMeshAgent.hasPath)
                return;

            if (!characterMaster.bodyInstance)
                return;

            movementVector = navMeshAgent.desiredVelocity.normalized;
        }

        private void SetNewTargetPosition()
        {
            if(currentTarget == null)
            {
                if (navMeshAgent.hasPath) //Let it finish its current path.
                    return;
            }

            var path = new NavMeshPath();
            navMeshAgent.CalculatePath(currentTarget.targetPosition, path);
            if (path.status == NavMeshPathStatus.PathInvalid)
                return;

            navMeshAgent.SetPath(path);
        }

        public class Target
        {
            public Vector3 targetPosition;
            public bool targetHasObject { get; }
            public GameObject targetObject { get;}

            public Target(GameObject targetObject)
            {
                targetHasObject = targetObject;
                targetPosition = targetObject.transform.position;
            }

            public Target(Vector3 position)
            {
                targetPosition = position;
            }
        }
    }
}
