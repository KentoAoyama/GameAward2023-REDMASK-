using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    /// <summary>
    /// Enemyの攻撃を制御するクラス（テスト用）<br/>
    /// 定期的にプレイヤーに向かって発砲する。
    /// </summary>
    public class TestEnemyAttackController : MonoBehaviour
    {
        [SerializeField]
        private float _fireInterval = 1f;
        [SerializeField]
        private GameObject _enemyBullet = default;

        private PlayerController _playerController = null;

        private void Start()
        {
            // ヒエラルキーからプレイヤーを取得する。
            _playerController = GameObject.FindObjectOfType<PlayerController>();
        }
        private float _timer = 0f;
        private void Update()
        {
            // 定期的に発砲する
            if (_timer > _fireInterval)
            {
                _timer = 0f;
                if (_enemyBullet != null)
                {
                    var bullet = Instantiate(_enemyBullet, transform.position, Quaternion.identity);
                    if (bullet.TryGetComponent(out EnemyBulletController bc))
                    {
                        bc.Setup(_playerController.transform.position - this.transform.position);
                    }
                }
            }
            _timer += Time.deltaTime;
        }
    }
}