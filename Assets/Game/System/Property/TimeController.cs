// 日本語対応
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

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


}
