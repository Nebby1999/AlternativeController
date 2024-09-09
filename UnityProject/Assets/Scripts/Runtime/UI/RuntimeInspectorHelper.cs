using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AC
{
    public class RuntimeInspectorHelper : MonoBehaviour
    {
        public GameObject hierarchy;
        public GameObject inspector;
        private void OnGUI()
        {
            if (Event.current.type == EventType.KeyUp)
            {
                if (Event.current.keyCode == KeyCode.F7)
                {
                    hierarchy.SetActive(!hierarchy.activeSelf);
                    inspector.SetActive(!inspector.activeSelf);
                }
            }
        }
    }
}
