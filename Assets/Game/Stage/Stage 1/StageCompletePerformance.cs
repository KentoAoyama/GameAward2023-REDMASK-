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
    private PlayableDirector _talkDirector = default;
    [SerializeField]
    private PlayableDirector _talkEndDirector = default;

    private bool _isTimeLineEnd = false;
    protected async override UniTask CompletePerformance()
    {
        //// タイムラインの再生を開始する
        //_talkDirector.Play();
        //_talkDirector.stopped += _ => _isTimeLineEnd = true;
        //// タイムラインの終了を検知したら処理を抜ける
        //await UniTask.WaitUntil(() => _isTimeLineEnd);
    }
    private void Update()
    {
        //if (!_isTimeLineEnd)
        //{
        //    _isTimeLineEnd = _playerController.InputManager.IsPressed[InputType.Fire1];
        //}
    }
}