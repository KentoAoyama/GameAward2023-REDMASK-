using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

/// <summary>
/// 指定した方向にまっすぐ飛ぶ敵弾のクラス
/// EnemyRifleクラスにプールされており、発射する際にアクティブになる
/// </summary>
public class EnemyBullet : MonoBehaviour
{
    [Header("弾の設定")]
    [SerializeField] private float _speed;
    [Tooltip("一定時間後に非アクティブになりプールに戻る")]
    [SerializeField] private float _lifeTime;

    private Stack<EnemyBullet> _pool;
    private Tween _tween;
    private Transform _transform;

    private void Awake()
    {
        _transform = transform;
    }

    private void OnDisable()
    {
        _tween?.Kill();
    }

    public void InitSetPool(Stack<EnemyBullet> pool) => _pool = pool;

    /// <summary>
    /// 発射される際にEnemyRifleクラスから呼び出される
    /// 引数の方向に飛び、時間経過でプールに戻す
    /// </summary>
    public void Shot(Vector3 dir)
    {
        Vector3 forward = dir.normalized;
        _tween = DOVirtual.DelayedCall(_lifeTime, ReturnPool)
            .OnUpdate(() => _transform.Translate(forward * _speed * GameManager.Instance.TimeController.EnemyTime));
    }

    /// <summary>
    /// プールに戻す
    /// 発射されてから一定時間後、もしくはプレイヤーにヒットした際に呼ばれる
    /// </summary>
    private void ReturnPool()
    {
        gameObject.SetActive(false);
        _pool?.Push(this);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && 
            collision.gameObject.TryGetComponent(out IDamageable damageable))
        {
            damageable.Damage();
            ReturnPool();
        }
    }
}
