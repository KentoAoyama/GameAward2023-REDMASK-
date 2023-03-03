// 日本語対応
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// プレイヤー死亡時の演出を制御するクラス
/// </summary>
public abstract class PlayerDeadEventBase : MonoBehaviour
{
    [Tooltip("プレイヤー死亡時に実行するイベント"), SerializeField]
    private UnityEvent _onPlayerDead = default;
    [Tooltip("死亡演出が完了したときに実行するイベント"), SerializeField]
    private UnityEvent _onComplete = default;

    public UnityEvent OnPlayerDead => _onPlayerDead;
    public UnityEvent OnComplete => _onComplete;

    private void Awake()
    {
        // シーン起動時に死亡演出は不要であるため自身を非アクティブにする。
        this.gameObject.SetActive(false);
    }
    private async void Start()
    {
        // プレイヤー死亡時に実行するイベントを発行する
        _onPlayerDead?.Invoke();
        // ゲームの状態を更新する
        GameManager.Instance.GameModeManager.ChangeGameMode(GameMode.PlayerDead);
        // プレイヤー死亡演出を再生する
        await PlayerDeadPerformance();
        // 演出完了時処理を発行する
        _onComplete?.Invoke();
    }
    /// <summary>
    /// 継承先で独自のプレイヤー死亡時演出を実装してください
    /// </summary>
    /// <returns></returns>
    protected abstract UniTask PlayerDeadPerformance();
}
