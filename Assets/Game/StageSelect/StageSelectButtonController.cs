// 日本語対応
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// ステージ選択画面上で ステージ選択ボタンの制御をするクラス。
/// </summary>
public class StageSelectButtonController : MonoBehaviour
{
    [Tooltip("ステージ選択用ボタンたち"), SerializeField]
    private SceneTransitionButtonForStageSelect[] _buttons = default;
    [SerializeField]
    private GameObject[] _completedImage = default;
    [SerializeField]
    private EventSystem _eventSystem = default;

    private GameObject _previousSelectedGameObject = default;

    private void Start()
    {
        // クリア済みステージ +1 だけアクティブに、
        // そうでないステージは 非アクティブにする。
        for (int i = 0; i < _buttons.Length; i++)
        {
            _buttons[i].Button.enabled = i == GameManager.Instance.CompletedStageManager.GetMaxCompletedStageNumber();
            if (i == GameManager.Instance.CompletedStageManager.GetMaxCompletedStageNumber())
            {
                _eventSystem.SetSelectedGameObject(_buttons[i].gameObject);
                _previousSelectedGameObject = _buttons[i].gameObject;
            }
            _buttons[i].KnifeParent.SetActive(i == GameManager.Instance.CompletedStageManager.GetMaxCompletedStageNumber());
        }
        for (int i = 0; i < _completedImage.Length; i++)
        {
            // バッテン済みの画像の処理
            _completedImage[i].SetActive(i < GameManager.Instance.CompletedStageManager.GetMaxCompletedStageNumber());
        }
    }
    private void Update()
    {
        if (_eventSystem.currentSelectedGameObject == null)
        {
            _eventSystem.SetSelectedGameObject(_previousSelectedGameObject);
        }
        _previousSelectedGameObject = _eventSystem.currentSelectedGameObject;
    }
}
