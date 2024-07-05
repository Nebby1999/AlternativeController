using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace AC
{

    public class InputHandler : MonoBehaviour
    {
        private RawImage _rawImage;
        private TMP_Text _textMeshPro;
    
        [SerializeField] private Color _colorOnPositive = Color.green;
        [SerializeField] private Color _colorOnNegative = Color.gray;
        [SerializeField] private KeyCode _keyCode; // Referencia del input
        [SerializeField] private bool _isPressed;
        private void Awake()
        {
            _rawImage = GetComponentInChildren<RawImage>();
            _textMeshPro = GetComponentInChildren<TMP_Text>();
        }
        private void Update()
        {
            ReadValue();
            UpdateDisplay();
        }
        private void ReadValue()
        {
            if(Input.GetKeyDown(_keyCode)) _isPressed = true;
            if(Input.GetKeyUp(_keyCode)) _isPressed = false;
        }
        private void UpdateDisplay()
        {
            _rawImage.color = _isPressed ? _colorOnPositive : _colorOnNegative;
            _textMeshPro.text = _keyCode.ToString(); // Asignar el nombre del KeyCode al TextMeshPro
        }
    }
}