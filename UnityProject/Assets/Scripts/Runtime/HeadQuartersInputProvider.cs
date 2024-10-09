using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

namespace AC
{
    /// <summary>
    /// Un provider el cual consigue los inputs del <see cref="HeadQuarters"/>
    /// </summary>
    [RequireComponent(typeof(PlayerInput))]
    public class HeadQuartersInputProvider : MonoBehaviour
    {
        /// <summary>
        /// El PlayerInput asociado a este componente
        /// </summary>
        public PlayerInput playerInput { get; private set; }

        private bool _isUsingKeyboardScheme;
        private bool _obtainedActions = false;

#if DEBUG
        [Tooltip("El vehiculo 1 esta en estado de combate?")]
        [SerializeField]
#endif
        private bool _isInBattleState1;
#if DEBUG
        [Tooltip("El vehiculo 2 esta en estado de combate?")]
        [SerializeField]
#endif
        private bool _isInBattleState2;

#if DEBUG
        [Tooltip("Los recursos rojos a que base deben ir?")]
        [SerializeField, Range(-1, 1)]
#endif
        private int _redResourceRouting;

#if DEBUG
        [Tooltip("Los recursos negros a que base deben ir?")]
        [SerializeField, Range(-1, 1)]
#endif
        private int _blackResourceRouting;

        private void Awake()
        {
            playerInput = GetComponent<PlayerInput>();
        }
        
        /// <summary>
        /// Consigue los input actuales del Headquarters
        /// </summary>
        /// <returns><see cref="HQInput"/></returns>
        public HQInput GetInputs()
        {
            return new HQInput()
            {
                vehicle1CombatMode = _isInBattleState1,
                vehicle2CombatMode = _isInBattleState2,
                redResourceDestination = _redResourceRouting,
                blackResourceDestination = _blackResourceRouting
            };
        }

        /// <summary>
        /// Estructura que representa los inputs del Headquarters
        /// </summary>
        public struct HQInput
        {
            /// <summary>
            /// Revisa si el vehiculo 1 esta en modo de combate o no
            /// </summary>
            public bool vehicle1CombatMode { get; init; }

            /// <summary>
            /// El vehiculo 2 esta en modo de combate?
            /// </summary>
            public bool vehicle2CombatMode { get; init; }

            /// <summary>
            /// A que base el recurso rojo va
            /// </summary>
            public int redResourceDestination { get; init; }

            /// <summary>
            /// A que base el recurso negro va
            /// </summary>
            public int blackResourceDestination { get; init; }
        }
    }
}