using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class EnemyLifeController : MonoBehaviour//, IDamageable
    {
        [Tooltip("死亡時に生成するオブジェクト。この敵の死亡演出。"), SerializeField]
        private GameObject _deathEffect = default;

        public virtual void Damage(float value)
        {
            if (_deathEffect != null)
            {
                Instantiate(_deathEffect, transform.position, Quaternion.identity);
            }
            
            Destroy(this.gameObject);
        }
    }
}