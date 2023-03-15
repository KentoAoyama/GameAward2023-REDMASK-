using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

/// <summary>
/// 視界の制御
/// </summary>
public class SightBehavior : MonoBehaviour
{
    [SerializeField] SightSensor _sightSensor;

    ReactiveProperty<bool> _isDetected = new();

    void Awake()
    {
        // 毎フレーム呼ばれるメソッド
        // trueが返ったら発見/falseで未発見
        // trueが返ったらPlayerFind/falseが返ったらPlayerHide
        // 

        // 毎フレーム視界の機能を呼んでそれをりあぷろで検知、めせぶろでメッセージングする
    }
}
