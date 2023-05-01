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
    
    private Color _defaultColor = new Color();

    private void Awake()
    {
        _defaultColor = _buttonText.color;
    }

    private void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == this.gameObject)
        {
            _buttonText.color = _selectedColor;
        }
        else
        {
            _buttonText.color = _defaultColor;
        }
    }
}
