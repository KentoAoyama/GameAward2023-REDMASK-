// 日本語対応
using System;
using UnityEngine;

namespace Enemy
{
    /// <summary>
    /// エネミーを生成するクラス
    /// </summary>
    public class TestEnemyGenerator : MonoBehaviour
    {
        [SerializeField]
        private GameObject _product = default;
        [SerializeField]
        private Transform _generatePos = default;

        public void GenerateEnemy()
        {
            Instantiate(_product, _generatePos.position, Quaternion.identity);
        }
    }
}