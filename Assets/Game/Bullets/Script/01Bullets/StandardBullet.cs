using UnityEngine;

namespace Bullet
{
    [System.Serializable]
    public class StandardBullet : BulletBase
    {
        public override BulletType Type => BulletType.StandardBullet;

        protected override void OnHitCollision(Collision2D target)
        {

        }

        protected override void OnHitTrigger(Collider2D target)
        {
            // ダメージを加える
            if (target.TryGetComponent(out IDamageable hit))
            {
                hit.Damage(_attackPower);

                // 弾の消滅処理
                if (_maxEnemyHitNumber <= 0)
                {
                    return;
                } // _maxEnemyHitNumber が0以下であれば、弾は無数の敵を貫く。
                else
                {
                    _currentEnemyHitNumber++;

                    if (_currentEnemyHitNumber >= _maxEnemyHitNumber)
                    {
                        // 自身を破棄する
                        Destroy(this.gameObject);
                    }
                } // _maxEnemyHitNumber が1以上であれば、弾はその数だけ敵を貫く。
            }
        }
    }
}