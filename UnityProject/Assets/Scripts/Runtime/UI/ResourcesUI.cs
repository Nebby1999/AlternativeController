using System.Text;
using TMPro;
using UnityEngine;

namespace AC
{
    public class ResourcesUI : MonoBehaviour, IHUDElement
    {
        [Tooltip("If this array is empty, then ALL teh resources within the manager are printed.")]
        public ResourceDef[] resourcesToPrint;
        [Tooltip("The resources manager that this behaviour reads")]
        public ResourcesManager tiedResources;
        [Tooltip("The Text component that displays the resources")]
        public TextMeshProUGUI textMesh;
        public HUD parentHud { get; set; }

        private string[] _printSelectedArray;
        private string[] _printAllArray;
        private string _formattingString;

        private void Awake()
        {
            _printAllArray = new string[ResourceCatalog.resourceCount];
            _printSelectedArray = new string[resourcesToPrint.Length];
        }
        private void Start()
        {
            _formattingString = string.Format(textMesh.text, tiedResources.name);
            _formattingString += "\n{0}";
        }

        //I dislike working with strings honestly... oh well
        private void Update()
        {
            if(resourcesToPrint.Length == 0)
            {
                PrintAll();
                return;
            }

            for(int i = 0; i < resourcesToPrint.Length; i++)
            {
                ResourceDef def = resourcesToPrint[i];
                _printSelectedArray[i] = string.Format("{0} Count: {1}", def.resourceName, tiedResources.GetResourceCount(def).ToString("0.0###"));
            }
            textMesh.text = string.Format(_formattingString, string.Join
                ("\n", _printSelectedArray));
        }

        private void PrintAll()
        {
            for(ResourceIndex index = (ResourceIndex)0; index < (ResourceIndex)ResourceCatalog.resourceCount; index++)
            {
                ResourceDef def = ResourceCatalog.GetResourceDef(index);
                _printAllArray[(int)index] = string.Format("{0} Count: {1}", def.resourceName, tiedResources.GetResourceCount(index).ToString("0.0###"));
            }
            textMesh.text = string.Format(_formattingString, string.Join("\n", _printAllArray));
        }
    }
}