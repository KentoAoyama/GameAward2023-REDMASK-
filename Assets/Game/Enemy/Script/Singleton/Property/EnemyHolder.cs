// 日本語対応
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    /// <summary>
    /// 全てのエネミーやアクティブなエネミーを管理提供するクラス
    /// </summary>
    public class EnemyHolder
    {
        private List<EnemyHolderRegister> _activeEnemyHolder = new List<EnemyHolderRegister>();
        private List<EnemyHolderRegister> _aliveEnemyholder = new List<EnemyHolderRegister>();

        /// <summary> 現在アクティブなエネミーを保持するリスト </summary>
        public List<EnemyHolderRegister> ActiveEnemyHolder => _activeEnemyHolder;
        /// <summary> 現在ヒエラルキーに存在する全てのエネミーを保持するリスト </summary>
        public List<EnemyHolderRegister> AliveEnemyholder => _aliveEnemyholder;
    }
}
