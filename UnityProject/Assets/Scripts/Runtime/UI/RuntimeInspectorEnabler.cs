using UnityEngine;

namespace AC
{
    /// <summary>
    /// Clase base que maneja la activacion del Runtime Inspector.
    /// </summary>
    public class RuntimeInspectorEnabler : MonoBehaviour, IHUDElement
    {
        public HUD parentHud { get; set; }

        /// <summary>
        /// El objeto que tiene el <see cref="RuntimeInspectorNamespace.RuntimeInspector"/>
        /// </summary>
        public GameObject runtimeInspector;

        /// <summary>
        /// El objeto que tiene el <see cref="RuntimeInspectorNamespace.RuntimeHierarchy"/>
        /// </summary>
        public GameObject runtimeHierarchy;

        /// <summary>
        /// El boton que debe ser presionado para activar o desactivar el Runtime Inspector y Hierarchy
        /// </summary>
        public KeyCode toggleKeyCode = KeyCode.F7;

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