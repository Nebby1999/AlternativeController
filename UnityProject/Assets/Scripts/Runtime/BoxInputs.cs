using UnityEngine;

namespace AC
{
    public class BoxInputs : MonoBehaviour
    {
        [SerializeField] private KeyCode[] _playerSwitches;
        [SerializeField] private KeyCode[] _cables;
        public bool GetPlayerSwitch(int index)
        {
            return Input.GetKey(_playerSwitches[index]);
        }
        public bool GetCableInput(int index)
        {
            return Input.GetKey(_cables[index]);
        }
    }
}
