using UnityEngine;

/// <summary>
/// 近接攻撃の武器のクラス
/// Enemy_RangeAttackオブジェクトが使用する
/// </summary>
public class EnemyMeleeWeapon : MonoBehaviour, IEnemyWeapon
{
    [Header("攻撃範囲")]
    [SerializeField] private float _radius;
    [Header("プレイヤーが属するレイヤー")]
    [SerializeField] private LayerMask _playerLayerMask;

    /// <summary>
    /// プレイヤーのみを検出するので長さは1で良い
    /// </summary>
    private Collider2D[] _results = new Collider2D[1];

    public void Attack()
    {
        int hitCount = Physics2D.OverlapCircleNonAlloc(transform.position, _radius, _results, _playerLayerMask);
        if (hitCount > 0)
        {
            _results[0].GetComponent<IDamageable>().Damage();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _radius);
    }
}
