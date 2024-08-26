using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

namespace AC
{
    [RequireComponent(typeof(PlayerInput))]
    public class HeadQuartersInputProvider : MonoBehaviour
    {
        public PlayerInput playerInput { get; private set; }

        private bool _isUsingKeyboardScheme;
        private bool _obtainedActions = false;

#if DEBUG
        [SerializeField]
#endif
        private bool _isInBattleState1;
#if DEBUG
        [SerializeField]
#endif
        private bool _isInBattleState2;

#if DEBUG
        [SerializeField, Range(-1, 1)]
#endif
        private int _redResourceRouting;

#if DEBUG
        [SerializeField, Range(-1, 1)]
#endif
        private int _blackResourceRouting;

        private void Awake()
        {
            playerInput = GetComponent<PlayerInput>();
        }

#if DEBUG
        private void OnGUI()
        {
            /*_isInBattleState1 = GUILayout.Toggle(_isInBattleState1, "Is in Battle State 1");
            _isInBattleState2 = GUILayout.Toggle(_isInBattleState2, "Is in Battle State 2 (UNUSED)");
            GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
            GUILayout.Label("Black Resource Routing: " + _blackResourceRouting);
            _blackResourceRouting = Mathf.RoundToInt(GUILayout.HorizontalScrollbar(_blackResourceRouting, 0.25f, -1, 1));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Red Resource Routing: " + _redResourceRouting);
            _redResourceRouting = Mathf.RoundToInt(GUILayout.HorizontalScrollbar(_redResourceRouting, 0.25f, -1, 1));
            GUILayout.EndHorizontal();*/
        }
#endif

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

        public struct HQInput
        {
            public bool vehicle1CombatMode { get; init; }
            public bool vehicle2CombatMode { get; init; }

            public int redResourceDestination { get; init; }
            public int blackResourceDestination { get; init; }
        }
    }
}