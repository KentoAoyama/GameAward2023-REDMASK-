// 日本語対応
using Cysharp.Threading.Tasks;
using Player;
using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary> 会話演出を制御 </summary>
public class Talk : MonoBehaviour
{
    [Tooltip("描画するテキストコンポーネント"), SerializeField]
    private Text _viewArea = default;
    [Tooltip("会話テキスト"), SerializeField]
    private TextAsset _talkData = default;
    [Tooltip("起動時から実行するかどうか"), SerializeField]
    private bool _playOnAwake = default;
    [Tooltip("連打防止時間（入力無効時間）"), SerializeField]
    private float _inputInvalidTime = 1f;
    [Tooltip("入力コンポーネント（ここは後で修正したい）"), SerializeField]
    private PlayerController _playerController = default;

    private bool _alive = false;
    private int _currentIndex = 0;
    private string[] _talkTexts = default;

    public event Action OnComplete = null;

    private void Awake()
    {
        gameObject.SetActive(_playOnAwake);
    }
    private void Start()
    {
        _talkTexts = _talkData.text.Split('\n');
        _alive = true;
        TextControl();
    }
    private async void TextControl()
    {
        // 入力が発生するたびにテキストを更新する。
        while (_alive)
        {
            try
            {
                // テキストをビューに渡す
                _viewArea.text = _talkTexts[_currentIndex];

                // 指定された秒数,入力を受け付けない
                await UniTask.Delay((int)(1000f * _inputInvalidTime),
                    cancellationToken: gameObject.GetCancellationTokenOnDestroy());

                // 入力を待機する。ゲームオブジェクトが破棄されたら UniTaskも破棄する。
                await UniTask.WaitUntil(() =>
                    _playerController.InputManager.IsPressed[InputType.Jump],
                    cancellationToken: gameObject.GetCancellationTokenOnDestroy());

                // インデックスを更新する
                _currentIndex++;
                if (_currentIndex >= _talkTexts.Length)
                {
                    _alive = false;
                    gameObject.SetActive(false);
                    _viewArea.text = "";
                } // 終了処理
            }
            catch (IndexOutOfRangeException e)
            {
                Debug.LogError(e.Message);
                Debug.LogError($"添字が範囲外を指定しました！値 :{_currentIndex}");
                _alive = false; // 無限ループ防止
                gameObject.SetActive(false);
                _viewArea.text = "";
            }
            catch (OperationCanceledException)
            {
                // UniTask破棄
                _alive = false; // 無限ループ防止
            }
            catch (Exception)
            {
                // 上記のエラー以外
                _alive = false; // 無限ループ防止
            }
        }
        OnComplete?.Invoke();
    }
    public void ClearIndex()
    {
        _currentIndex = 0;
    }
}
