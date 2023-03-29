// 日本語対応
using Cysharp.Threading.Tasks;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

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
    [SerializeField]
    private Image _behindObjDontTouchiImage = default;

    /// <summary> 現在表示している場所 </summary>
    private ScreenArea _currentScreenArea = ScreenArea.Left;
    /// <summary> 入力を無効にするかどうか </summary>
    private bool _canScroll = true;
    /// <summary> 自身のRectTransform </summary>
    private RectTransform _rectTransform = null;
    /// <summary> DOTween保存用 </summary>
    private TweenerCore<Vector3, Vector3, VectorOptions> _slidingAnim = default;

    private void Awake()
    {
        _behindObjDontTouchiImage.gameObject.SetActive(false);
        _rectTransform = GetComponent<RectTransform>();
    }
    private async void OnEnable()
    {
        await UniTask.WaitUntil(() => _prepareInputManager != null);
        _prepareInputManager.PrepareInputController.UI.Navigate.performed += InputTracking;
    }
    private void OnDisable()
    {
        _prepareInputManager.PrepareInputController.UI.Navigate.performed -= InputTracking;
    }
    private void OnDestroy()
    {
        // このオブジェクトを破棄する際にDOTweenをキルする。
        // （警告を発生させない為の処理）
        _slidingAnim?.Kill();
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
                AnimStart();
                _currentScreenArea = ScreenArea.Right;
                _slidingAnim = _rectTransform.DOLocalMoveX(_rightAreaPos, _duration).
                    SetEase(_slidingEasing).
                    OnComplete(AnimEnd);
            }
            else if (value < 0f && _currentScreenArea == ScreenArea.Right)
            {
                AnimStart();
                _currentScreenArea = ScreenArea.Left;
                _slidingAnim = _rectTransform.DOLocalMoveX(_leftAreaPos, _duration).
                    SetEase(_slidingEasing).
                    OnComplete(AnimEnd);
            }
        }
        void AnimStart()
        {
            _canScroll = false;
            _behindObjDontTouchiImage.gameObject.SetActive(true);
        }
        void AnimEnd()
        {
            _canScroll = true;
            _behindObjDontTouchiImage.gameObject.SetActive(false);
        }
    }
    public enum ScreenArea
    {
        Right,
        Left,
    }
}
