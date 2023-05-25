// 日本語対応
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerDeadCanvasController : MonoBehaviour
{
    [SerializeField]
    private GameObject _firstSelectedButton = default;

    [SerializeField]
    private GameObject _playerUIObject = default;

    private GameObject _preSelectedButton = default;

    private void OnEnable()
    {
        _playerUIObject.SetActive(false);
        EventSystem.current.SetSelectedGameObject(_firstSelectedButton);
    }
    private void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            EventSystem.current.SetSelectedGameObject(_preSelectedButton);
        }
        _preSelectedButton = EventSystem.current.currentSelectedGameObject;
    }
}
