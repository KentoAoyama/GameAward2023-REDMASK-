using DG.Tweening;
using UnityEngine;

/// <summary>
/// このクラスを用いて時間経過によるステートの遷移を行う
/// </summary>
public class TransitionWithTimeElapsed : MonoBehaviour
{
    [Header("ステートの遷移までの遅延時間")]
    [SerializeField] private float _minDelay = 2.0f;
    [SerializeField] private float _maxDelay = 3.0f;

    private Tween _tween;

    private void OnDisable()
    {
        _tween?.Kill();
    }

    /// <summary>
    /// 一定時間後に時間経過で遷移をさせるためのメッセージを送信する
    /// 時間経過で遷移するステートに遷移した際に呼ばれる
    /// </summary>
    public void DelayedSendTransitionMessage(StateTransitionMessenger messageSender)
    {
        if (_tween != null)
        {
            _tween.Kill();
            Debug.LogWarning("重複して呼ばれたので以前呼ばれた処理をキャンセルします");
        }

        float delayTime = Random.Range(_minDelay, _maxDelay);
        _tween = DOVirtual.DelayedCall(delayTime, () =>
        {
            messageSender.SendMessage(StateTransitionTrigger.TimeElapsed);
        }, ignoreTimeScale: false);
    }
}
