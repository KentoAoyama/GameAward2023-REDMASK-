// 日本語対応
using UnityEngine;

/// <summary>
/// ステージ選択画面上で ステージ選択ボタンの制御をするクラス。
/// </summary>
public class StageSelectButtonController : MonoBehaviour
{
    [Tooltip("ステージ選択用ボタンたち"), SerializeField]
    private GameObject[] _buttons = default;

    private void Awake()
    {
        // クリア済みステージ +1 だけアクティブに、
        // そうでないステージは 非アクティブにする。
        for (int i = 0; i < _buttons.Length; i++)
        {
            _buttons[i].SetActive(i <= GameManager.Instance.
                CompletedStageManager.GetMaxCompletedStageNumber());
        }
    }
}
