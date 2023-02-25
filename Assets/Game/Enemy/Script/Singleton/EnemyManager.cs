// 日本語対応
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class EnemyManager
    {
        #region Singleton
        private static EnemyManager _instance = new EnemyManager();
        public static EnemyManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    Debug.LogError($"Error! Please correct!");
                }
                return _instance;
            }
        }
        private EnemyManager() { }
        #endregion

        private EnemyHolder _enemyHolder = new EnemyHolder();

        public EnemyHolder EnemyHolder => _enemyHolder;
    }
}