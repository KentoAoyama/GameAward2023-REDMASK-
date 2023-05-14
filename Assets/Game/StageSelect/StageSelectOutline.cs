// 日本語対応
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StageSelectOutline : MonoBehaviour
{
    private Outline outline = null;
    private void Start()
    {
        outline = GetComponent<Outline>();
    }
    private void Update()
    {
        outline.enabled = EventSystem.current.currentSelectedGameObject == this.gameObject;
    }
}
