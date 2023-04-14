// 日本語対応
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

/// <summary>
/// ステージのタイムラインを制御する用のコンポーネント
/// </summary>
public class StageTimelineController : MonoBehaviour
{
    [Header("次のシーンの名前")]
    [SceneName, SerializeField]
    private string _nextSceneName = default;
    [Header("開始 演出用タイムライン")]
    [SerializeField]
    private PlayableDirector _firstPerformanceDirector = default;
    [Header("会話 演出用タイムライン")]
    [SerializeField]
    private PlayableDirector _secondPerformanceEndDirector = default;
    [Header("終了 演出用タイムライン")]
    [SerializeField]
    private PlayableDirector _thirdPerformanceEndDirector = default;

    /// <summary> 開始タイムライン検知用フラグ </summary>
    private bool _isFirstTimeLineEnd = false;
    /// <summary> 終了タイムライン検知用フラグ </summary>
    private bool _isThirdTimeLineEnd = false;

    private async void Awake()
    {
        // 開始 タイムラインを再生する。
        _firstPerformanceDirector.Play();
        _firstPerformanceDirector.stopped += _ => _isFirstTimeLineEnd = true;
        await UniTask.WaitUntil(() => _isFirstTimeLineEnd); // 開始タイムラインが終了するのを待つ。
        // 会話タイムラインを再生する。
        _secondPerformanceEndDirector.Play();
        await UniTask.WaitUntil(() =>
            Mouse.current.leftButton.wasPressedThisFrame ||      // 発砲ボタンが押下されるのを待つ。
            Gamepad.current.rightShoulder.wasPressedThisFrame);  // （左クリックかゲームパッドのRightShoulder）
        _firstPerformanceDirector.Stop();
        // 終了タイムラインを再生する。
        _secondPerformanceEndDirector.Play();
        _secondPerformanceEndDirector.stopped += _ => _isThirdTimeLineEnd = true;
        await UniTask.WaitUntil(() => _isThirdTimeLineEnd); // 終了タイムラインが終了するのを待つ。

        // 設定されたシーンを読み込む。
        SceneManager.LoadScene(_nextSceneName);
    }
}
