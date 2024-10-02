using UnityEngine;

namespace AC
{
    public class RuntimeInspectorEnabler : MonoBehaviour, IHUDElement
    {
        public HUD parentHud { get; set; }

        public GameObject runtimeInspector;
        public GameObject runtimeHierarchy;
        public KeyCode toggleKeyCode;

        private void OnGUI()
        {
            if(Event.current.type == EventType.KeyDown)
            {
                if(Event.current.keyCode == toggleKeyCode)
                {
                    runtimeInspector.SetActive(!runtimeInspector.activeSelf);
                    runtimeHierarchy.SetActive(!runtimeHierarchy.activeSelf);
                }
            }
        }
    }
}