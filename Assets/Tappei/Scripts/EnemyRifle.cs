using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵の銃
/// </summary>
public class EnemyRifle : MonoBehaviour, IEnemyWeapon
{
    [Header("敵弾")]
    [SerializeField] private EnemyBullet _enemyBullet;
    [Header("プールする敵弾の数")]
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
        // オブプーから弾を呼び出す

    }
}
