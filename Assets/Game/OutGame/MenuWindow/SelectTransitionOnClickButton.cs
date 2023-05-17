// 日本語対応
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectTransitionOnClickButton : MonoBehaviour
{
    [SerializeField]
    private EventSystem _eventSystem = default;
    [SerializeField]
    private GameObject _nextSelectButton = default;
    private void Awake()
    {
        var myButton = GetComponent<Button>();
        myButton.onClick.AddListener(OnClick);
    }
    private void OnClick()
    {
        EventSystem.current.SetSelectedGameObject(_nextSelectButton.gameObject);
    }
}
