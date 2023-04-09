using Cysharp.Threading.Tasks;
using System.Threading;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 台詞を表示させるのに使用するクラス
/// 受け取る側
/// </summary>
public class LineMessageReceiver : MonoBehaviour
{
    [Header("台詞を表示するテキスト")]
    [SerializeField] private Text _text;
    [Header("表示する台詞を更新する間隔(秒)")]
    [SerializeField] private float _interval = 1.0f;

    private string _nextLine;

    private void Awake()
    {
        // 一番最後に受信したメッセージが次の台詞に反映される
        MessageBroker.Default.Receive<LineMessage>().Subscribe(message =>
        {
            _nextLine = message.Line;

        }).AddTo(this);
    }

    private void Start()
    {
        LineControlAsync(this.GetCancellationTokenOnDestroy()).Forget();
    }

    /// <summary>
    /// 台詞を受信したらテキストに表示してそのテキストが非表示になるのを待つという動作を繰り返す
    /// </summary>
    private async UniTaskVoid LineControlAsync(CancellationToken token)
    {
        while (true)
        {
            token.ThrowIfCancellationRequested();

            await UniTask.WaitUntil(() => _nextLine != string.Empty, cancellationToken: token);
            string line = _nextLine;
            _nextLine = string.Empty;
            await SetTextAsync(line, token);
        }
    }

    private async UniTask SetTextAsync(string line, CancellationToken token)
    {
        _text.text = line;
        await UniTask.Delay(System.TimeSpan.FromSeconds(_interval), cancellationToken: token);
        _text.text = string.Empty;
    }
}
