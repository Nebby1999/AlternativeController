using UnityEngine;
using UnityEngine.EventSystems;

namespace AC
{
    /// <summary>
    /// Clase que representa un HeadsUpDisplay
    /// </summary>
    [RequireComponent(typeof(Canvas))]
    public class HUD : UIBehaviour
    {
        /// <summary>
        /// Todos los HUDElements que hay en el HUD
        /// </summary>
        public IHUDElement[] hudElements { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            hudElements = GetComponentsInChildren<IHUDElement>();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            foreach(var hudElement in hudElements)
            {
                hudElement.parentHud = this;
            }
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            foreach(var hudElement in hudElements)
            {
                hudElement.parentHud = null;
            }
        }
    }
}