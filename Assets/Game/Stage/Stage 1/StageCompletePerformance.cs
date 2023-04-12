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
    [Header("完了演出開始"), SerializeField]
    private PlayableDirector _completePerformanceDirector = default;
    [SerializeField]
    private PlayableDirector _completePerformanceEndDirector = default;

    private bool _isTimeLineEnd = false;
    protected async override UniTask CompletePerformance()
    {
        //// タイムラインの再生を開始する
        //_talkDirector.Play();
        //_talkDirector.stopped += _ => _isTimeLineEnd = true;
        //// タイムラインの終了を検知したら処理を抜ける
        //await UniTask.WaitUntil(() => _isTimeLineEnd);
    }
}