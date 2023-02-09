using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 弾の挙動を制御するクラス
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public abstract class BulletControllerBase : MonoBehaviour
{
    [Tooltip("弾の生存時間"), SerializeField]
    private float _lifeTime = 1f;
    [SerializeField]
    private float _moveSpeed = 6f;
    [SerializeField]
    protected float _attackPower = 1f;

    private Rigidbody2D _rigidbody2D = null;
    private Vector2 _shootAngle = default;
    private Collider2D[] _nonCollisionTarget = null;

    /// <summary>
    /// セットアップ処理
    /// </summary>
    /// <param name="shootAngle"> 射出角度 </param>
    public void Setup(Vector2 shootAngle, Collider2D[] nonCollisionTarget)
    {
        _shootAngle = shootAngle;
        _nonCollisionTarget = nonCollisionTarget;
    }

    private void Start()
    {
        // 指定した方向、速度で弾を移動させる。
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _rigidbody2D.velocity = _shootAngle.normalized * _moveSpeed;
        // 指定した時間経過したら、自身を破棄する。
        Destroy(this.gameObject, _lifeTime);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        foreach (var e in _nonCollisionTarget)
        {
            if (e == collision) return;
        } // 非接触対象は無視する

        // 接触時処理
        OnHit(collision);
    }
    /// <summary>
    /// 接触時の処理<br/>
    /// この関数内で、敵に当たった時の処理、壁に接触した時の処理、等を記述してください。
    /// </summary>
    /// <param name="target"></param>
    protected abstract void OnHit(Collider2D target);
}