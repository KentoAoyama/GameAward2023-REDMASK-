using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// このクラスを用いて時間経過によるステートの遷移を行う
/// つ川に
/// </summary>
public class TransitionTimer
{

    //[Header("ステートの遷移までの遅延時間(秒)")]
    //[SerializeField] private float _minDelay = 2.0f;
    //[SerializeField] private float _maxDelay = 3.0f;

    // TODO:仮のタイムスピードの値になっている
    private float _timeSpeed = 1;

    private CancellationTokenSource _cts;

    private void OnDisable()
    {
        _cts?.Cancel();
    }

    /// <summary>
    /// 外部から呼び出す際、コールバックに遷移処理を渡すことで一定時間後の遷移を行う
    /// このメソッドの実行中に再度呼び出すと以前の処理はキャンセルされる
    /// </summary>
    public void TimeElapsedExecute(UnityAction callback)
    {
        ExecuteCancel();
        _cts = new();

        //float delayTime = Random.Range(_minDelay, _maxDelay);
        //TimerAsync(callback, delayTime).Forget();
    }

    /// <summary>
    /// Update()のタイミングでタイムスピードに応じた値を足すことで
    /// スローモーションやポーズに対応させている
    /// </summary>
    private async UniTaskVoid TimerAsync(UnityAction callback, float delayTime)
    {
        _cts.Token.ThrowIfCancellationRequested();
        
        float total = 0;
        while (total <= delayTime)
        {
            total += _timeSpeed * Time.deltaTime;
            await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken: _cts.Token);
        }

        callback?.Invoke();
    }

    public void ExecuteCancel() => _cts?.Cancel();
}
