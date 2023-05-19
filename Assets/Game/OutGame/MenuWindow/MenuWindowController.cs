// 日本語対応
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuWindowController : MonoBehaviour
{
    [SerializeField]
    private GameObject _firstSelectedObject = default;
    [SerializeField]
    private Button[] _mainButtons = default;

    private GameObject _previousSelectedObject = null;

    private void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(_firstSelectedObject);
        _previousSelectedObject = _firstSelectedObject;
        if (_firstSelectedObject.TryGetComponent(out Outline currentOutline))
        {
            currentOutline.enabled = true;
        }
    }
    private void OnDisable()
    {
        for (int i = 0; i < _mainButtons.Length; i++)
        {
            if (_mainButtons[i].TryGetComponent(out Outline outline))
            {
                outline.enabled = false;
            }
        }
    }
    private void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            EventSystem.current.SetSelectedGameObject(_previousSelectedObject);
        }
        if (EventSystem.current.currentSelectedGameObject != _previousSelectedObject)
        {
            if (EventSystem.current.currentSelectedGameObject.TryGetComponent(out Outline currentOutline))
            {
                currentOutline.enabled = true;
            }
            if (_previousSelectedObject.TryGetComponent(out Outline previousOutline))
            {
                previousOutline.enabled = false;
            }
        }
        _previousSelectedObject = EventSystem.current.currentSelectedGameObject;
    }

    public void EnableMainButtons()
    {
        for (int i = 0; i < _mainButtons.Length; i++)
        {
            _mainButtons[i].interactable = true;
        }
    }
    public void DisableMainButtons()
    {
        for (int i = 0; i < _mainButtons.Length; i++)
        {
            _mainButtons[i].interactable = false;
        }
    }
}
