using UnityEngine;

/// <summary>
/// 敵弾
/// </summary>
public class EnemyBullet : MonoBehaviour
{
    [Header("弾速")]
    [SerializeField] private float _speed;

    private Transform _transform;
    private Vector3 _dir;

    private void Awake()
    {
        _transform = transform;
    }

    /// <summary>
    /// 飛ばす方向の設定、内部で正規化している
    /// </summary>
    public void Init(Vector3 dir)
    {
        _dir = dir.normalized;
    }

    private void Update()
    {
        _transform.Translate(_dir * _speed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && 
            collision.gameObject.TryGetComponent(out IDamageable damageable))
        {
            damageable.Damage();
        }
    }
}
