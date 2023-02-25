using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

namespace Enemy
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class EnemyBulletController : MonoBehaviour
    {
        [SerializeField]
        private float _shootPower = 1f;
        [SerializeField]
        private float _attackPower = 1f;

        private Vector2 _shootAngle = default;

        public void Setup(Vector2 shootAngle)
        {
            _shootAngle = shootAngle;
        }
        private void Start()
        {
            var rb2D = GetComponent<Rigidbody2D>();
            rb2D.velocity = _shootAngle.normalized * _shootPower;
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out PlayerController player))
            {
                player.LifeController.Damage(_attackPower);
            }
            Destroy(this.gameObject);
        }
    }
}