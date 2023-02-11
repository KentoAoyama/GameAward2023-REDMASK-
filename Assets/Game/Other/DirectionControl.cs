using System.Collections;
using UnityEngine;

/// <summary>
/// 水平移動方向を表すクラス
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

    // 原点を取得保存する
    public void Init(Transform transform)
    {
        _origin = transform;
        _previousPositionX = _origin.position.x;
    }
    public void Update()
    {
        // 位置が変わっている時だけ更新する
        if (Mathf.Abs(_previousPositionX - _origin.position.x) > 0.01f)
        {
            if (_previousPositionX > _origin.position.x)
            {
                MovementDirectionX = Constant.Right;

                var s = _origin.localScale;
                s.x *= s.x > 0f ? -1f : 1f;
                _origin.localScale = s;
            } // 右に移動している時の場合
            else
            {
                MovementDirectionX = Constant.Left;

                var s = _origin.localScale;
                s.x *= s.x < 0f ? -1f : 1f;
                _origin.localScale = s;
            } // 左に移動している時の場合
        }
        // 次フレーム用に値を保存する
        _previousPositionX = _origin.position.x;
    }
}
