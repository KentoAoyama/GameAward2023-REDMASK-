using DG.Tweening;
using UnityEngine;

/// <summary>
/// このクラスを用いて時間経過によるステートの遷移を行う
/// </summary>
public class EnemyTransitionTimer : MonoBehaviour
{
    [Header("ステートの遷移までの遅延時間")]
    [SerializeField] private float _minDelay;
    [SerializeField] private float _maxDelay;

    private Tween _tween;

    private void OnDisable()
    {
        _tween?.Kill();
    }

    /// <summary>
    /// 一定時間後に時間経過で遷移をさせるためのメッセージを送信する
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
