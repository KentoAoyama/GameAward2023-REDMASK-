// 日本語対応
using Cysharp.Threading.Tasks;
using Player;
using UnityEngine;
using UnityEngine.Playables;

/// <summary>
/// ステージ開始演出用コンポーネント
/// </summary>
public class StageStartPerformance : StageStartEventBase
{
    [SerializeField]
    private PlayerController _playerController = default;
    [Header("完了演出開始"), SerializeField]
    private PlayableDirector _startPerformanceDirector = default;
    [SerializeField]
    private PlayableDirector _startPerformanceEndDirector = default;

    private bool _isTimeLineEnd = false;

    protected async override UniTask StartPerformance()
    {
        _startPerformanceDirector.Play();
        await UniTask.WaitUntil(() => _playerController.InputManager.IsPressed[InputType.Fire1]);
        _startPerformanceDirector.Stop();
        _startPerformanceEndDirector.Play();
        _startPerformanceEndDirector.stopped += _ => _isTimeLineEnd = true;
        await UniTask.WaitUntil(() => _isTimeLineEnd);
    }
}
