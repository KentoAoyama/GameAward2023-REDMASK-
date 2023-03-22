using UnityEngine;

/// <summary>
/// “G’e
/// </summary>
public class EnemyBullet : MonoBehaviour
{
    [Header("’e‘¬")]
    [SerializeField] private float _speed;

    private Transform _transform;
    private Vector3 _dir;

    private void Awake()
    {
        _transform = transform;
    }

    /// <summary>
    /// ”ò‚Î‚·•ûŒü‚Ìİ’èA“à•”‚Å³‹K‰»‚µ‚Ä‚¢‚é
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
            damageable.Damage(0);
        }
    }
}
