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
        private InputAction _battleState1;

#if DEBUG
        [SerializeField]
#endif
        private bool _isInBattleState1;
        private InputAction _battleState2;
#if DEBUG
        [SerializeField]
#endif
        private bool _isInBattleState2;


        private InputAction _redResourceToBase1;
        private InputAction _redResourceRest;
        private InputAction _redResourceToBase2;
#if DEBUG
        [SerializeField, Range(-1, 1)]
#endif
        private int _redResourceRouting;

        private InputAction _blackResourceToBase1;
        private InputAction _blackResourceRest;
        private InputAction _blackResourceToBase2;
#if DEBUG
        [SerializeField, Range(-1, 1)]
#endif
        private int _blackResourceRouting;

        private void Awake()
        {
            playerInput = GetComponent<PlayerInput>();
        }

        private void OnEnable()
        {
            playerInput.ActivateInput();
            if (!_obtainedActions)
                GetActions();

            _isUsingKeyboardScheme = playerInput.currentControlScheme == "Keyboard";
#if DEBUG
            if(_isUsingKeyboardScheme)
            {
                _battleState1.performed += _battleState1_performed;
                _battleState2.performed += _battleState2_performed;

                _redResourceToBase1.performed += _redResourceToBase1_performed;
                _redResourceRest.performed += _redResourceRest_performed;
                _redResourceToBase2.performed += _redResourceToBase2_performed;

                _blackResourceToBase1.performed += _blackResourceToBase1_performed;
                _blackResourceRest.performed += _blackResourceRest_performed;
                _blackResourceToBase2.performed += _blackResourceToBase2_performed;
            }
#endif
        }

        private void OnDisable()
        {
            _battleState1.performed -= _battleState1_performed;
            _battleState2.performed -= _battleState2_performed;

            _redResourceToBase1.performed -= _redResourceToBase1_performed;
            _redResourceRest.performed -= _redResourceRest_performed;
            _redResourceToBase2.performed -= _redResourceToBase2_performed;

            _blackResourceToBase1.performed -= _blackResourceToBase1_performed;
            _blackResourceRest.performed -= _blackResourceRest_performed;
            _blackResourceToBase2.performed -= _blackResourceToBase2_performed;
        }

        #region Debug Bindings
#if DEBUG
        private void _battleState2_performed(InputAction.CallbackContext obj)
        {
            _isInBattleState2 = !_isInBattleState2;
        }

        private void _battleState1_performed(InputAction.CallbackContext obj)
        {
            _isInBattleState1 = !_isInBattleState1;
        }
        private void _redResourceToBase2_performed(InputAction.CallbackContext obj)
        {
            _redResourceRouting = 1;
        }

        private void _redResourceRest_performed(InputAction.CallbackContext obj)
        {
            _redResourceRouting = -0;
        }

        private void _redResourceToBase1_performed(InputAction.CallbackContext obj)
        {
            _redResourceRouting = -1;
        }

        private void _blackResourceToBase2_performed(InputAction.CallbackContext obj)
        {
            _redResourceRouting = 1;
        }

        private void _blackResourceRest_performed(InputAction.CallbackContext obj)
        {
            _redResourceRouting = -0;
        }

        private void _blackResourceToBase1_performed(InputAction.CallbackContext obj)
        {
            _redResourceRouting = -1;
        }
#endif
        #endregion

        private void GetActions()
        {
            _obtainedActions = true;
            var map = playerInput.currentActionMap;

            _battleState1 = map.FindAction(ACInputActionGUIDS.HQMap.switchToBattleState1GUID);
            _battleState2 = map.FindAction(ACInputActionGUIDS.HQMap.switchToBattleState2GUID);

            _redResourceToBase1 = map.FindAction(ACInputActionGUIDS.HQMap.redResourceToBase1GUID);
            _redResourceRest = map.FindAction(ACInputActionGUIDS.HQMap.redResourceRestGUID);
            _redResourceToBase2 = map.FindAction(ACInputActionGUIDS.HQMap.redResourceToBase2GUID);

            _blackResourceToBase1 = map.FindAction(ACInputActionGUIDS.HQMap.blackResourceToBase1GUID);
            _blackResourceRest = map.FindAction(ACInputActionGUIDS.HQMap.blackResourceToRestGUID);
            _blackResourceToBase2 = map.FindAction(ACInputActionGUIDS.HQMap.blackResourceToBase2GUID);
        }

        private void Update()
        {
            //Keyboard schemes is handled via C# actions.
            if (_isUsingKeyboardScheme)
                return;

            _isInBattleState1 = _battleState1.phase == InputActionPhase.Performed;
            _isInBattleState2 = _battleState2.phase == InputActionPhase.Performed;

            _redResourceRouting = GetRouting(_redResourceToBase1, _redResourceRest, _redResourceToBase2);
            _blackResourceRouting = GetRouting(_blackResourceToBase1, _blackResourceRest, _blackResourceToBase2);
        }

        private int GetRouting(InputAction minusOne, InputAction zero, InputAction one)
        {
            if (minusOne.phase == InputActionPhase.Performed)
                return -1;

            if (zero.phase == InputActionPhase.Performed)
                return 0;

            if (one.phase == InputActionPhase.Performed)
                return 1;

            return 0;
        }

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