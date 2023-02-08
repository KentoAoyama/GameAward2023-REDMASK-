using System.Collections;
using UnityEngine;

/// <summary>
/// x座標の移動方向を制御するクラス
/// </summary>
[System.Serializable]
public class DirectionControl
{
    /// <summary> 原点 </summary>
    private Transform _origin = null;
    /// <summary> 前フレームのx座標 </summary>
    private float _previousPositionX = default;

    /// <summary> 移動方向を 1fか -1fで表す </summary>
    public float MovementDirectionX { get; private set; } = Constant.Right;

    public void Init(Transform transform)
    {
        _origin = transform;
    }
    public void Update()
    {
        if (Mathf.Abs(_previousPositionX - _origin.position.x) > 0.01f)
        {
            if (_previousPositionX > _origin.position.x)
            {
                MovementDirectionX = Constant.Right;
            }
            else
            {
                MovementDirectionX = Constant.Left;
            }
        }

        _previousPositionX = _origin.position.x;
    }
}
