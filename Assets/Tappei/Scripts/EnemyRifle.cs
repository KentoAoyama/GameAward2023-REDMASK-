using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵の銃
/// </summary>
public class EnemyRifle : MonoBehaviour, IEnemyWeapon
{
    [Header("発射する弾の設定")]
    [SerializeField] private EnemyBullet _enemyBullet;
    [Tooltip("プールする敵弾の数、攻撃頻度を上げる場合はこちらも上げないといけない")]
    [SerializeField] private int _poolQuantity;
    [Header("敵弾を発射するマズル")]
    [Tooltip("弾はマズルの位置からマズルの前方向に飛んでいく")]
    [SerializeField] private Transform _muzzle;

    private Stack<EnemyBullet> _pool;

    private void Awake()
    {
        _pool = new Stack<EnemyBullet>(_poolQuantity);
        CreatePool();
    }

    private void CreatePool()
    {
        for (int i = 0; i < _poolQuantity; i++)
        {
            EnemyBullet bullet = Instantiate(_enemyBullet, transform.position, Quaternion.identity);
            bullet.gameObject.SetActive(false);
            _pool.Push(bullet);
        }
    }

    /// <summary>
    /// プールから取り出す処理
    /// 戻す処理は弾側に持たせている
    /// </summary>
    private EnemyBullet PopPool()
    {
        if (_pool.Count > 0)
        {
            EnemyBullet bullet = _pool.Pop();
            bullet.gameObject.SetActive(true);
            return bullet;
        }
        else
        {
            return null;
        }
    }

    public void Attack()
    {
        EnemyBullet bullet = PopPool();
        bullet.transform.position = _muzzle.position;
        bullet.Shot(_muzzle.forward);
    }
}
