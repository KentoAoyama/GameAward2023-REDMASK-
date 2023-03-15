using UnityEngine;

namespace Bullet
{
    /// <summary>
    /// 貫通弾クラス
    /// </summary>
    [System.Serializable]
    public class PenetrateBullet : BulletBase
    {
        [TagName, SerializeField]
        private string _wallTagName = default;
        [SerializeField]
        private int _maxWallHitNumber = 1;

        private int _wallHitCount = 0;

        public int MaxWallHitNumber => _maxWallHitNumber;

        public override BulletType Type => BulletType.PenetrateBullet;

        protected override void OnHitCollision(Collision2D target)
        {

        }

        protected override void OnHitTrigger(Collider2D target)
        {
            // ダメージを加える
            if (target.TryGetComponent(out IDamageable hit))
            {
                hit.Damage(_attackPower);
                return;
            }
            if (target.tag == _wallTagName)
            {
                _wallHitCount++;
                if (_wallHitCount > _maxWallHitNumber)
                {
                    Destroy(this.gameObject);
                }
            }
        }
    }
}