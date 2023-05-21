using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

/// <summary>
/// 指定した方向にまっすぐ飛ぶ敵弾のクラス
/// EnemyRifleクラスにプールされており、発射する際にアクティブになる
/// </summary>
public class EnemyBullet : MonoBehaviour, IPausable, IDamageable
{
    [Header("ヒットするタグの設定")]
    [Tooltip("プレイヤーのタグ")]
    [SerializeField, TagName] private string PlayerTagName;
    [Tooltip("壁などのステージ内のオブジェクトのタグ")]
    [SerializeField, TagName] private string WallTagName;
    [Header("弾の設定")]
    [SerializeField] private float _speed;
    [SerializeField] private Transform _sprite;

    private Transform _transform;
    private Stack<EnemyBullet> _pool;
    private Vector3 _velocity;
    private float _time;
    private bool _isPause;

    private void Awake()
    {
        InitOnAwake();
    }

    private void InitOnAwake()
    {
        this.OnEnableAsObservable().Subscribe(_ => GameManager.Instance.PauseManager.Register(this));
        this.OnDisableAsObservable().Subscribe(_ => GameManager.Instance.PauseManager.Lift(this));

        _transform = transform;

        // 一定時間前方に飛んでプールに戻る
        this.UpdateAsObservable().Where(_ => !_isPause).Subscribe(_ =>
        {
            float deltaTime = Time.deltaTime * GameManager.Instance.TimeController.EnemyTime;
            // 途中で消えて違和感あるようだったらこの値を大きくする
            float lifeTime = 3.0f;

            _time += deltaTime;
            if (_time > lifeTime)
            {
                ReturnPool();
            }
            else
            {
                _sprite.right = _velocity;
                _transform.Translate(_velocity * deltaTime);
            }
        });

        // プレイヤーにヒットしたらプールに戻る
        this.OnTriggerEnter2DAsObservable().Subscribe(c =>
        {
            if (c.CompareTag(PlayerTagName) && c.gameObject.TryGetComponent(out IDamageable damageable))
            {
                damageable.Damage();
                ReturnPool();
            }
            else if (c.CompareTag(WallTagName))
            {
                ReturnPool();
            }
        });
    }

    /// <summary>
    /// 弾が生成された際にEnemyRifleクラスから呼び出される
    /// </summary>
    public void InitSetPool(Stack<EnemyBullet> pool) => _pool = pool;
    /// <summary>
    /// 発射される際にEnemyRifleクラスから呼び出される
    /// </summary>
    public void SetVelocity(Vector3 dir) => _velocity = dir * _speed;
    /// <summary>
    /// このメソッドを呼ぶことでプールに戻す
    /// </summary>
    private void ReturnPool()
    {
        _time = 0;
        gameObject.SetActive(false);
        _pool?.Push(this);
    }

    public void Pause() => _isPause = true;
    public void Resume() => _isPause = false;
    public void Damage() => ReturnPool();
}
