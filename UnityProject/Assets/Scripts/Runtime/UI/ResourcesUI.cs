using System.Text;
using TMPro;
using UnityEngine;

namespace AC
{
    /// <summary>
    /// Behaviour que maneja la UI de un REsourcesManager, usado tanto para mostrar los recursos de las bases y del HQ
    /// </summary>
    public class ResourcesUI : MonoBehaviour, IHUDElement
    {
        [Tooltip("Si este arreglo esta vacio, entonces TODOS los recursos dentro del ResourceManager se mostraran en la UI.")]
        public ResourceDef[] resourcesToPrint;
        [Tooltip("El ResourcesManager que este behaviour lee")]
        public ResourcesManager tiedResources;
        [Tooltip("El texto en si que muestra la informacion a los jugadores.")]
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

        //Odio trabajar con strings... ya que...
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