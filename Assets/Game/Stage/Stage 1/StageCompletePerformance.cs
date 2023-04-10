// 日本語対応
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using Player;

public class StageCompletePerformance : StageCompleteEventBase
{
    [SerializeField]
    private PlayerController _playerController = default;
    [SerializeField]
    private PlayableDirector _playableDirector = default;
    [SerializeField]
    private TimelineAsset _timeline = default;

    private bool _isTimeLineEnd = false;
    protected async override UniTask CompletePerformance()
    {
        // タイムラインの再生を開始する
        _playableDirector.Play();
        _playableDirector.stopped += _ => _isTimeLineEnd = true;
        // タイムラインの終了を検知したら処理を抜ける
        await UniTask.WaitUntil(() => _isTimeLineEnd);
    }
    private void Update()
    {
        if (!_isTimeLineEnd)
        {
            _isTimeLineEnd = _playerController.InputManager.IsPressed[InputType.Fire1];
        }
    }
}