// 日本語対応
using Cysharp.Threading.Tasks;
using Player;
using UnityEngine;
using UnityEngine.Playables;

/// <summary>
/// ステージ完了時演出用コンポーネント
/// </summary>
public class StageCompletePerformance : StageCompleteEventBase
{
    [SerializeField]
    private PlayerController _playerController = default;
    [SerializeField]
    private PlayableDirector _completePerformanceDirector = default;
    [SerializeField]
    private PlayableDirector _completePerformanceEndDirector = default;

    private bool _isTimeLineEnd = false;

    protected async override UniTask CompletePerformance()
    {
        _completePerformanceDirector.Play();
        await UniTask.WaitUntil(() => _playerController.InputManager.IsPressed[InputType.Fire1]);
        _completePerformanceDirector.Stop();
        _completePerformanceEndDirector.Play();
        _completePerformanceEndDirector.stopped += _ => _isTimeLineEnd = true;
        await UniTask.WaitUntil(() => _isTimeLineEnd);
    }
}