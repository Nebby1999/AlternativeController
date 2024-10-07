using Nebula.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AC
{
    public class AIDriver : MonoBehaviour
    {
        [Tooltip("A name for this driver for identification purposes.")]
        public string driverName;
        public IAITargetSelector targetSelector { get; private set; }

        [Header("Selection Configuration")]
        [Tooltip("The type of target for this driver, this is how the target for the baseAI will be found if this driver is selected.")]
        [SerializableSystemType.RequiredBaseType(typeof(IAITargetSelector))]
        [SerializeField] private SerializableSystemType _targetType;
        [Tooltip("If the Target Type fails to select a proper target, this driver will not be selected")]
        public bool requiresTarget;
        [Tooltip("The BaseAI needs to have this skill slot for this driver to be selected. Use none if no skill slot is required.")]
        public SkillSlot requiredSlot;
        [Tooltip("For this to be selected the BaseAI needs to have LOS to it's target.")]
        public bool selectionRequiresLOSToTarget;

        public float minDistanceSqr => minDistance * minDistance;
        [Tooltip("The minimum distance from the target required for this behaviour")]
        public float minDistance;

        public float maxDistanceSqr => maxDistance * maxDistance;
        [Tooltip("The maximum distance from the target required for this behaviour")]
        public float maxDistance = float.PositiveInfinity;

        [Header("Behaviour")]
        [Tooltip("If true, the AI will use navmesh to reach it's target, otherwise, it'll go on a straight line.")]
        public bool useNavMeshForPathing;
        [Tooltip("How the AI Driver will press its buttons for activating skills.")]
        public ButtonPressType buttonPressType;

        private void Awake()
        {
            targetSelector = (IAITargetSelector)Activator.CreateInstance((Type)_targetType);
            targetSelector.Initialize(this);
        }

        public enum ButtonPressType
        {
            Abstain,
            SinglePress,
            Hold
        }
    }
}
