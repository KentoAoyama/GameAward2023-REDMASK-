// 日本語対応
using Cysharp.Threading.Tasks;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UniRx;

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
    [SerializeField]
    private Image _leftSideArrowIamge = default;
    [SerializeField]
    private Image _rightSideArrowIamge = default;
    [SerializeField]
    private Text _leftSideManualTextIamge = default;
    [SerializeField]
    private Text _rightSideManualTextIamge = default;

    /// <summary> 現在表示している場所 </summary>
    private ReactiveProperty<ScreenArea> _currentScreenArea = new ReactiveProperty<ScreenArea>(ScreenArea.Left);
    /// <summary> 入力を無効にするかどうか </summary>
    private bool _canScroll = true;
    /// <summary> 自身のRectTransform </summary>
    private RectTransform _rectTransform = null;
    /// <summary> DOTween保存用 </summary>
    private TweenerCore<Vector3, Vector3, VectorOptions> _slidingAnim = default;
    /// <summary> DOTween保存用 </summary>
    private TweenerCore<Color, Color, ColorOptions> _leftSideArrowAnim = default;
    /// <summary> DOTween保存用 </summary>
    private TweenerCore<Color, Color, ColorOptions> _rightSideArrowAnim = default;
    /// <summary> DOTween保存用 </summary>
    private TweenerCore<Color, Color, ColorOptions> _leftSideManualTextAnim = default;
    /// <summary> DOTween保存用 </summary>
    private TweenerCore<Color, Color, ColorOptions> _rightSideManualTextAnim = default;

    public bool IsAnimationNow { get => !_canScroll; }
    public IReadOnlyReactiveProperty<ScreenArea> CurrentScreenArea => _currentScreenArea;

    private void Awake()
    {
        _behindObjDontTouchiImage.gameObject.SetActive(false);
        _rectTransform = GetComponent<RectTransform>();
    }
    private async void OnEnable()
    {
        await UniTask.WaitUntil(() => _prepareInputManager != null && _prepareInputManager.PrepareInputController != null);

        _prepareInputManager.PrepareInputController.Prepare.LeftScroll.started += LeftScroll;
        _prepareInputManager.PrepareInputController.Prepare.RightScroll.started += RightScroll;
    }
    private void OnDisable()
    {
        _prepareInputManager.PrepareInputController.Prepare.LeftScroll.started -= LeftScroll;
        _prepareInputManager.PrepareInputController.Prepare.RightScroll.started -= RightScroll;
    }
    private void OnDestroy()
    {
        // このオブジェクトを破棄する際にDOTweenをキルする。
        // （警告を発生させない為の処理）
        _slidingAnim?.Kill();
    }
    private void RightScroll(InputAction.CallbackContext action)
    {
        // スクロール中は無視
        if (!_canScroll) return;
        // 現在既に右の場合も無視。
        if (_currentScreenArea.Value == ScreenArea.Right) return;

        RightScrollAnimStart();
        _currentScreenArea.Value = ScreenArea.Right;
        _slidingAnim = _rectTransform.DOLocalMoveX(_rightAreaPos, _duration).
            SetEase(_slidingEasing).
            OnComplete(RightScrollAnimEnd);
    }
    private void LeftScroll(InputAction.CallbackContext action)
    {
        // スクロール中は無視
        if (!_canScroll) return;
        // 現在既に左の場合も無視。
        if (_currentScreenArea.Value == ScreenArea.Left) return;

        LeftScrollAnimStart();
        _currentScreenArea.Value = ScreenArea.Left;
        _slidingAnim = _rectTransform.DOLocalMoveX(_leftAreaPos, _duration).
            SetEase(_slidingEasing).
            OnComplete(LeftScrollAnimEnd);
    }

    private void RightScrollAnimStart()
    {
        // 左側の矢印,テキストのアルファ値を0にする。
        _leftSideArrowAnim?.Kill();
        _leftSideManualTextAnim?.Kill();
        _leftSideArrowAnim = _leftSideArrowIamge.DOFade(0f, _duration).OnComplete(() => _leftSideArrowAnim = null);
        _leftSideManualTextAnim = _leftSideManualTextIamge.DOFade(0f, _duration).OnComplete(() => _leftSideManualTextAnim = null);

        _canScroll = false;
        _behindObjDontTouchiImage.gameObject.SetActive(true);
    }
    private void LeftScrollAnimStart()
    {
        // 右側の矢印,テキストのアルファ値を0にする。
        _rightSideArrowAnim?.Kill();
        _rightSideManualTextAnim?.Kill();
        _rightSideArrowAnim = _rightSideArrowIamge.DOFade(0f, _duration).OnComplete(() => _rightSideArrowAnim = null);
        _rightSideManualTextAnim = _rightSideManualTextIamge.DOFade(0f, _duration).OnComplete(() => _rightSideManualTextAnim = null);

        _canScroll = false;
        _behindObjDontTouchiImage.gameObject.SetActive(true);
    }
    private void RightScrollAnimEnd()
    {
        // 右側の矢印,テキストのアルファ値を1にする。
        _rightSideArrowAnim?.Kill();
        _rightSideManualTextAnim?.Kill();
        _rightSideArrowAnim = _rightSideArrowIamge.DOFade(1f, _duration).OnComplete(() => _rightSideArrowAnim = null);
        _rightSideManualTextAnim = _rightSideManualTextIamge.DOFade(1f, _duration).OnComplete(() => _rightSideManualTextAnim = null);

        _canScroll = true;
        _behindObjDontTouchiImage.gameObject.SetActive(false);
    }
    private void LeftScrollAnimEnd()
    {
        // 左側の矢印,テキストのアルファ値を1にする。
        _leftSideArrowAnim?.Kill();
        _leftSideManualTextAnim?.Kill();
        _leftSideArrowAnim = _leftSideArrowIamge.DOFade(1f, _duration).OnComplete(() => _leftSideArrowAnim = null);
        _leftSideManualTextAnim = _leftSideManualTextIamge.DOFade(1f, _duration).OnComplete(() => _leftSideManualTextAnim = null);

        _canScroll = true;
        _behindObjDontTouchiImage.gameObject.SetActive(false);
    }
    public enum ScreenArea
    {
        Right,
        Left,
    }
}
