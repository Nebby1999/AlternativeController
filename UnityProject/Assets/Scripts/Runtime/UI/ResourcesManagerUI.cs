using Nebula;
using TMPro;
using UnityEngine;

namespace AC.UI
{
    public class ResourcesManagerUI : MonoBehaviour
    {
        public TextMeshProUGUI redResourcesText;
        public TextMeshProUGUI blackResourcesText;
        public CanvasGroup canvasGroup;
        public ResourcesManager tiedManager
        {
            get => _tiedManager;
            set
            {
                if(_tiedManager != value)
                {
                    if (value)
                        value.onEmpty -= OnResourcesEmptied;
                    _tiedManager = value;
                    _tiedManager.onEmpty += OnResourcesEmptied;
                }
            }
        }
        private ResourcesManager _tiedManager;

        private ResourceDef _redResource;
        private ResourceDef _blackResource;

        private void Start()
        {
            _redResource = ResourceCatalog.GetResourceDef(ResourceCatalog.FindResource("Red"));
            _blackResource = ResourceCatalog.GetResourceDef(ResourceCatalog.FindResource("Black"));

            redResourcesText.color = _redResource.resourceColor;
            redResourcesText.outlineColor = _redResource.resourceColor.GetBestOutline();

            blackResourcesText.color = _blackResource.resourceColor;
            blackResourcesText.outlineColor = _blackResource.resourceColor.GetBestOutline();
        }

        private void OnResourcesEmptied()
        {
            tiedManager = null;
            canvasGroup.alpha = 0;
        }

        private void Update()
        {
            if (!tiedManager)
                return;

            redResourcesText.SetText("Red Resources: {0}", tiedManager.GetResourceCount(_redResource));
            blackResourcesText.SetText("Black Resources: {0}", tiedManager.GetResourceCount(_blackResource));
        }

        private void OnDestroy()
        {
            Destroy(redResourcesText.material);
            Destroy(blackResourcesText.material);
        }
    }
}