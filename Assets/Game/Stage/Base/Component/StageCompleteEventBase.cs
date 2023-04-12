// 日本語対応
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

/// <summary>
/// ステージクリア時の演出を制御するクラスの基底クラス
/// </summary>
public abstract class StageCompleteEventBase : MonoBehaviour
{
    [SceneName, SerializeField]
    private string _stageSelectName = default;
    [Tooltip("ステージクリア時に実行するイベント"), SerializeField]
    private UnityEvent _onStageComplete = default;
    [Tooltip("ステージクリア演出が完了したときに実行するイベント"), SerializeField]
    private UnityEvent _onPerformanceComplete = default;

    public UnityEvent OnStageComplete => _onStageComplete;
    public UnityEvent OnComplete => _onPerformanceComplete;

    private async void Start()
    {
        // ポーズする
        GameManager.Instance.PauseManager.ExecutePause();
        // ステージクリア時に実行するイベントを発行する
        _onStageComplete?.Invoke();
        // ゲームの状態を更新する
        GameManager.Instance.GameModeManager.ChangeGameMode(GameMode.Complete);
        // ステージクリア演出が完了するまで待機する
        await CompletePerformance();
        // 演出完了時処理を発行する
        _onPerformanceComplete?.Invoke();

        // 稼働中のDOTweenを全て破棄し、ステージ選択シーンへ遷移する。
        DOTween.KillAll();
        //GameManager.Instance.BulletsCountManager.
        SceneManager.LoadScene(_stageSelectName);
    }
    /// <summary>
    /// 継承先で独自のステージクリア演出を実装してください。
    /// </summary>
    protected abstract UniTask CompletePerformance();
}