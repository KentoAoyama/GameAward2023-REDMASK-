// 日本語対応
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public abstract class StageStartEventBase : MonoBehaviour
{
    [Tooltip("ステージ開始時に実行するイベント"), SerializeField]
    private UnityEvent _onStageStart = default;
    [Tooltip("ステージ開始演出完了時に実行するイベント"), SerializeField]
    private UnityEvent _onComplete = default;

    public UnityEvent OnStageStart => _onStageStart;
    public UnityEvent OnComplete => _onComplete;

    private async void Start()
    {
        // ステージ開始時用イベントを発行する
        _onStageStart?.Invoke();
        // ゲームの状態を更新する
        GameManager.Instance.GameModeManager.ChangeGameMode(GameMode.Start);
        // ステージ開始演出を再生する
        await StartPerformance();
        // ゲームの状態を更新する
        GameManager.Instance.GameModeManager.ChangeGameMode(GameMode.PlayGame);
        // ステージ演出完了時用イベントを発行する
        _onComplete?.Invoke();
    }
    /// <summary>
    /// 継承先で独自のステージ開始演出を実装してください
    /// </summary>
    protected abstract UniTask StartPerformance();
}