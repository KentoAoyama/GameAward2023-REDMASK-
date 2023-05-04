// 日本語対応
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UniRx;

public class PrepareUIController : MonoBehaviour
{
    [SerializeField]
    private PrepareImageSlideController _prepareAnimation = default;
    [SerializeField]
    private EventSystem _eventSystem = default;

    [SerializeField]
    private Button _rightFirstSelectedButton = default;
    [SerializeField]
    private Button[] _rightButtons = default;

    private GameObject _previousSelectedObj = null;

    private void Awake()
    {
        _prepareAnimation.CurrentScreenArea.Subscribe(ChangeScreenArea);
    }
    private void ChangeScreenArea(PrepareImageSlideController.ScreenArea screenArea)
    {
        if (screenArea == PrepareImageSlideController.ScreenArea.Left)
            OnLeft();
        else
            OnRiht();
    }
    private void OnLeft()
    {
        // ============= 左側の起動処理 ============= //

        // ============= 右側の停止処理 ============= //
        // 右側に配置されたボタンを全て無効にする
        for (int i = 0; i < _rightButtons.Length; i++)
        {
            _rightButtons[i].enabled = false;
        }
    }
    private void OnRiht()
    {
        // ============= 右側の起動処理 ============= //
        // 右側に配置されたボタンを全て有効にする
        for (int i = 0; i < _rightButtons.Length; i++)
        {
            _rightButtons[i].enabled = true;
        }
        // 右側に遷移したときに最初に選択しておきたいボタンを選択状態にする。
        _rightFirstSelectedButton.Select();
        // ============= 左側の停止処理 ============= //

    }
    private void Update()
    {
        // 選択オブジェクトがなくなったら前のやつを選択しなおす。
        if (_eventSystem.currentSelectedGameObject == null)
        {
            _eventSystem.SetSelectedGameObject(_previousSelectedObj);
        }
        _previousSelectedObj = _eventSystem.currentSelectedGameObject;
    }

    public GameObject GetNearSelectableObj(Transform origin)
    {
        for (int i = 0; i < _rightButtons.Length; i++)
        {
            // 自分を探す
            if (origin.gameObject == _rightButtons[i].gameObject)
            {
                // 自分と近い 選択可能な オブジェクトを探す
                // 近いとは登録されたインデックスの数値が近いことを指す
                for (int j = i + 1; j < _rightButtons.Length + i; j++)
                {
                    int index = j % _rightButtons.Length;
                    if (!_rightButtons[index].interactable) continue;
                    return _rightButtons[index].gameObject;
                }
                // 検索に失敗した場合 nullを返す。
                return null;
            }
        }

        return null;
    }
}
