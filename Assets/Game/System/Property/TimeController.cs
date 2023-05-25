// 日本語対応
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Cysharp.Threading.Tasks;
using System;

/// <summary>
/// 時間を制御するクラス
/// </summary>
public class TimeController
{
    public TimeController()
    {
        _timeInformation = Resources.Load<TimeInformation>("Time Information");
    }
    private TimeInformation _timeInformation = null;

    private float _playerTime = 1;

    private float _bulletTime = 1;

    private float _enemyTime = 1;

    private float _cameraTime = 1;

    private bool _isEmagencyStop = false;

    private bool _isPause = false;

    private bool _isTimeStop = false;

    public float PlayerTime => _playerTime;

    public float EnemyTime => _enemyTime;

    public float BulletTime => _bulletTime;

    public float CameraTime => _cameraTime;

    /// <summary> 現在の時間速度 </summary>
    private ReactiveProperty<float> _currentTimeScale = new ReactiveProperty<float>(1f);

    /// <summary> 現在の時間速度 </summary>
    public IReadOnlyReactiveProperty<float> CurrentTimeScale => _currentTimeScale;

    /// <summary> 時間の速度を変更する </summary>
    public void ChangeTimeSpeed(bool isSlow)
    {
        if (_isTimeStop) return;

        if (isSlow)
        {
            _playerTime = _timeInformation.PlayerSlowSpeed;
            _enemyTime = _timeInformation.EnemySlowSpeed;
            _bulletTime = _timeInformation.BulletSlowSpeed;
            _cameraTime = _timeInformation.CameraSpeed;
        }
        else
        {
            _playerTime = 1;
            _enemyTime = 1;
            _bulletTime = 1;
            _cameraTime = 1;
        }
    }

    ///// <summary>ヒットストップを強制終了させる</summary>
    //public void EmagencyStopHitStop()
    //{
    //    _isEmagencyStop = true;

    //    ChangeTimeSpeed(false);
    //}

    ///// <summary>ヒットストップ開始させる</summary>
    //public void HitStop()
    //{
    //    _isEmagencyStop = true;
    //    _isEmagencyStop = false;

    //    HitStopTimer(_timeInformation.HitStopTime, () => !_isPause, () => _isEmagencyStop, () => StartHitStop(), () => EndHitStop());
    //}

    ///// <summary>ヒットストップ開始の処理</summary>
    //public void StartHitStop()
    //{
    //    _isTimeStop = true;

    //    _playerTime = 0;
    //    _enemyTime = 0;
    //    _bulletTime = 0;
    //    _cameraTime = 0;
    //}

    ///// <summary>ヒットストップ終了の処理</summary>
    //public void EndHitStop()
    //{
    //    _isTimeStop = false;

    //    bool isSlow = Player.PlayerController.CurentPlayerController.GunSetUp.IsGunSetUp;

    //    ChangeTimeSpeed(isSlow);
    //}

    //public async void HitStopTimer(float time, Func<bool> isTimerUpdate = null, Func<bool> finishTrigger = null, Action startEvent = null, Action onComplete = null)
    //{
    //    startEvent?.Invoke();

    //    float timer = 0f;

    //    while (timer < time)
    //    {
    //        if (finishTrigger != null && finishTrigger())
    //        {
    //            return;
    //        }
    //        if (isTimerUpdate == null)
    //        {
    //            timer += Time.deltaTime;
    //        }
    //        else if (isTimerUpdate())
    //        {
    //            timer += Time.deltaTime;
    //        }

    //        Debug.Log(timer);
    //        await UniTask.DelayFrame(1, PlayerLoopTiming.Update);
    //    }

    //    Debug.Log("End");
    //    onComplete?.Invoke();
    //}

}
