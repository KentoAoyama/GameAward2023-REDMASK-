// 日本語対応
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// データロードクラス
/// </summary>
public class LoadButton : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(
            GameManager.Instance.SaveLoadManager.ExecuteLoad);
    }
}
