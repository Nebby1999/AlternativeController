using UnityEngine;
using UnityEngine.EventSystems;

namespace AC
{
    [RequireComponent(typeof(Canvas))]
    public class HUD : UIBehaviour
    {
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