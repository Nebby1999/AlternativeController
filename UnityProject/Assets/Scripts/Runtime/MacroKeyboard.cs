using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AC
{

    public class MacroKeyboard : MonoBehaviour, IDirigeable
    {
        [SerializeField] private KeyCode[] _actions;

        public int GetRotation()
        {
            return Input.GetKeyDown(_actions[4]) ? 1 : Input.GetKeyDown(_actions[3]) ? -1 : 0;
        }
        public bool GetActionDown(int index)
        {
            return Input.GetKeyDown(_actions[index]);
        }
        public bool GetAction(int index)
        {
            return Input.GetKey(_actions[index]);
        }
    }
}
