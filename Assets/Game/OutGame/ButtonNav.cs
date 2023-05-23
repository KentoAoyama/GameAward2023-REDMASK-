// 日本語対応
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonNav : MonoBehaviour
{
    [SerializeField]
    private GameObject _nextSelectedButton = default;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() => EventSystem.current.SetSelectedGameObject(_nextSelectedButton));
    }
}
