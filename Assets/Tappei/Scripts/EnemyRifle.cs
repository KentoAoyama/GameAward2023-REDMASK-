using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// “G‚Ìe
/// </summary>
public class EnemyRifle : MonoBehaviour, IEnemyWeapon
{
    [Header("“G’e")]
    [SerializeField] private EnemyBullet _enemyBullet;
    [Header("ƒv[ƒ‹‚·‚é“G’e‚Ì”")]
    [SerializeField] private int _poolQuantity;

    private Stack<EnemyBullet> _pool = new Stack<EnemyBullet>();

    private void Awake()
    {
        for (int i = 0; i < _poolQuantity; i++)
        {
            EnemyBullet bullet = Instantiate(_enemyBullet, transform.position, Quaternion.identity);
            bullet.transform.position = Vector3.one * 100;
            _pool.Push(bullet);
        }
    }

    public void Attack()
    {
        // ƒIƒuƒv[‚©‚ç’e‚ğŒÄ‚Ño‚·

    }
}
