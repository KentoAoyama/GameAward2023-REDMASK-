using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof (Button))]
public class ButtonSelectController : MonoBehaviour
{
    [SerializeField]
    Text _buttonText = default;
    [SerializeField]
    Color _selectedColor = Color.red;

    private Button _button;
    
    private Color _defaultColor = new Color();
    private Outline _textOutline = default;

    private void Awake()
    {
        _defaultColor = _buttonText.color;
        _textOutline = _buttonText.gameObject.GetComponent<Outline>();
        _button = GetComponent<Button>();
    }

    private void Update()
    {
        if (!_button.enabled) return;
        
        if (EventSystem.current.currentSelectedGameObject == this.gameObject)
        {
            _buttonText.color = _selectedColor;
            _textOutline.enabled = true;
        }
        else
        {
            _buttonText.color = _defaultColor;
            _textOutline.enabled = false;
        }
    }
}
