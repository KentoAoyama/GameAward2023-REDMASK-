using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 指定した方向にまっすぐ飛ぶ敵弾のクラス
/// EnemyRifleクラスにプールされており、発射する際にアクティブになる
/// </summary>
public class EnemyBullet : MonoBehaviour, IPausable
{
    /// <summary>途中で消えて違和感あるようだったらこの値を大きくする</summary>
    private static float LifeTime = 3.0f;

    [Header("ヒットするタグの設定")]
    [Tooltip("プレイヤーのタグ")]
    [SerializeField, TagName] private string PlayerTagName;
    [Tooltip("壁などのステージ内のオブジェクトのタグ")]
    [SerializeField, TagName] private string WallTagName;
    [Header("弾の設定")]
    [SerializeField] private float _speed;

    private Transform _transform;
    private Stack<EnemyBullet> _pool;
    private Vector3 _velocity;
    private float _timer;
    private bool _isPause;

    private void Awake()
    {
        _transform = transform;
    }

    private void OnEnable()
    {
        GameManager.Instance.PauseManager.Register(this);
    }

    private void OnDisable()
    {
        GameManager.Instance.PauseManager.Lift(this);
    }

    public void Pause() => _isPause = true;
    public void Resume() => _isPause = false;

    /// <summary>弾が生成された際にEnemyRifleクラスから呼び出される</summary>
    public void InitSetPool(Stack<EnemyBullet> pool) => _pool = pool;
    /// <summary>発射される際にEnemyRifleクラスから呼び出される</summary>
    public void SetVelocity(Vector3 dir) => _velocity = dir * _speed;

    void Update()
    {
        if (_isPause) return;

        float deltaTime = Time.deltaTime * GameManager.Instance.TimeController.EnemyTime;

        _timer += deltaTime;
        if (_timer > LifeTime)
        {
            ReturnPool();
        }
        else
        {
            _transform.Translate(_velocity * deltaTime);
        }
    }

    /// <summary>
    /// プールに戻す
    /// 発射されてから一定時間後、もしくはプレイヤーにヒットした際に呼ばれる
    /// </summary>
    private void ReturnPool()
    {
        _timer = 0;
        gameObject.SetActive(false);
        _pool?.Push(this);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(PlayerTagName) &&
            collision.gameObject.TryGetComponent(out IDamageable damageable))
        {
            damageable.Damage();
            ReturnPool();
        }
        else if (collision.CompareTag(WallTagName))
        {
            ReturnPool();
        }
    }
}
