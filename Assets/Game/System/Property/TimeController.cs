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
    /// <summary> 現在の時間速度 </summary>
    private ReactiveProperty<float> _currentTimeScale = new ReactiveProperty<float>(1f);

    /// <summary> 現在の時間速度 </summary>
    public IReadOnlyReactiveProperty<float> CurrentTimeScale => _currentTimeScale;

    /// <summary> 時間の速度を変更する </summary>
    public void ChangeTimeSpeed(float value)
    {
        if (value < 0f)
        {
            Debug.LogWarning($"無効な値が渡されました。\n渡された値 :{value}");
            return;
        }
        _currentTimeScale.Value = value;
    }
}
