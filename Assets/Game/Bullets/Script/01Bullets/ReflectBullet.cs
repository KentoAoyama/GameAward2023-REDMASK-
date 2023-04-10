using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Bullet
{
    /// <summary>
    /// 反射弾クラス
    /// </summary>
    [System.Serializable]
    public class ReflectBullet : BulletBase
    {
        [Tooltip("最大何回壁を反射するか"), SerializeField]
        private int _maxWallCollisionCount = 0;

        public override BulletType Type => BulletType.ReflectBullet;
        public int MaxWallCollisionCount => _maxWallCollisionCount;

        public async void ToStart(Vector2[] positions)
        {
            int index = 1;
            _rigidbody2D = GetComponent<Rigidbody2D>();
            while (index < positions.Length)
            {
                try
                {
                    _rigidbody2D.velocity = (Vector3)positions[index] - transform.position;

                    await WaitMove(transform, transform.position, positions[index]);
                    index++;
                }
                catch (ArgumentOutOfRangeException e)
                {
                    Debug.LogError(e.Message);
                    Debug.LogError($"リストの範囲外が指定されました。 値 :{index}");
                    return;
                }
                catch (OperationCanceledException)
                {
                    return;
                }
            }
            Destroy(this.gameObject);
        }
        protected override void OnHitTrigger(Collider2D target)
        {
            // ダメージを加える
            if (target.TryGetComponent(out IDamageable damageable))
            {
                damageable.Damage();

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

        protected override void OnHitCollision(Collision2D target)
        {

        }

        private async UniTask WaitMove(Transform origin, Vector2 startPos, Vector2 targetPos)
        {
            if (startPos.x < targetPos.x && // 右上
                startPos.y < targetPos.y)
            {
                await UniTask.WaitUntil(() =>
                    origin.position.x > targetPos.x &&
                    origin.position.y > targetPos.y,
                    cancellationToken: gameObject.GetCancellationTokenOnDestroy());
            }
            else if (startPos.x > targetPos.x && // 左上
                     startPos.y < targetPos.y)
            {
                await UniTask.WaitUntil(() =>
                    origin.position.x < targetPos.x &&
                    origin.position.y > targetPos.y,
                    cancellationToken: gameObject.GetCancellationTokenOnDestroy());
            }
            else if (startPos.x < targetPos.x && // 右下
                     startPos.y > targetPos.y)
            {
                await UniTask.WaitUntil(() =>
                    origin.position.x > targetPos.x &&
                    origin.position.y < targetPos.y,
                    cancellationToken: gameObject.GetCancellationTokenOnDestroy());
            }
            else if (startPos.x > targetPos.x && // 左下
                     startPos.y > targetPos.y)
            {
                await UniTask.WaitUntil(() =>
                    origin.position.x < targetPos.x &&
                    origin.position.y < targetPos.y,
                    cancellationToken: gameObject.GetCancellationTokenOnDestroy());
            }
            transform.position = targetPos;
        }
    }
}