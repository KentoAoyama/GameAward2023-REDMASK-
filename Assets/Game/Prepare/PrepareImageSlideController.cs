// 日本語対応
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class PrepareImageSlideController : MonoBehaviour
{
    [Tooltip("入力管理クラス"), SerializeField]
    private PrepareInputManager _prepareInputManager = default;
    [Tooltip("どのようにイージングするか"), SerializeField]
    private Ease _slidingEasing = default;
    [Tooltip("スライドに掛ける時間"), SerializeField]
    private float _duration = default;
    [Tooltip("左側の中心ポジション"), SerializeField]
    private float _leftAreaPos;
    [Tooltip("右側の中心ポジション"), SerializeField]
    private float _rightAreaPos;

    /// <summary> 現在表示している場所 </summary>
    private ScreenArea _currentScreenArea = ScreenArea.Left;
    /// <summary> 入力を無効にするかどうか </summary>
    private bool _canScroll = true;
    /// <summary> 自身のRectTransform </summary>
    private RectTransform _rectTransform = null;

    private async void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        await UniTask.WaitUntil(() => _prepareInputManager != null);
        _prepareInputManager.PrepareInputController.UI.Navigate.performed += InputTracking;
    }

    /// <summary>
    /// 入力を追跡し、条件に応じて
    /// </summary>
    /// <param name="action"></param>
    private void InputTracking(InputAction.CallbackContext action)
    {
        if (!_canScroll) return;
        float value;
        if (Mathf.Abs(value = action.ReadValue<float>()) > 0.5f)
        {
            if (value > 0f && _currentScreenArea == ScreenArea.Left)
            {
                _canScroll = false;
                _currentScreenArea = ScreenArea.Right;
                _rectTransform.DOLocalMoveX(_rightAreaPos, _duration).
                    SetEase(_slidingEasing).
                    OnComplete(() => _canScroll = true);
            }
            else if (value < 0f && _currentScreenArea == ScreenArea.Right)
            {
                _canScroll = false;
                _currentScreenArea = ScreenArea.Left;
                _rectTransform.DOLocalMoveX(_leftAreaPos, _duration).
                    SetEase(_slidingEasing).
                    OnComplete(() => _canScroll = true);
            }
        }
    }

    public enum ScreenArea
    {
        Right,
        Left,
    }
}
