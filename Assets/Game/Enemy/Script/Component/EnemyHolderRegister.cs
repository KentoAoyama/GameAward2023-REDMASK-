// 日本語対応
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    /// <summary>
    /// エネミーホルダーに自身を登録する。
    /// </summary>
    public class EnemyHolderRegister : MonoBehaviour
    {
        private void Awake()
        {
            if (!EnemyManager.Instance.EnemyHolder.AliveEnemyholder.Contains(this))
            {
                EnemyManager.Instance.EnemyHolder.AliveEnemyholder.Add(this);
            }
            else
            {
                Debug.LogWarning("既にリストに含まれています");
            }
        }
        private void OnDestroy()
        {
            if (EnemyManager.Instance.EnemyHolder.AliveEnemyholder.Contains(this))
            {
                EnemyManager.Instance.EnemyHolder.AliveEnemyholder.Remove(this);
            }
            else
            {
                Debug.LogWarning("リストに含まれていません。");
            }
        }

        private void OnEnable()
        {
            if (!EnemyManager.Instance.EnemyHolder.ActiveEnemyHolder.Contains(this))
            {
                EnemyManager.Instance.EnemyHolder.ActiveEnemyHolder.Add(this);
            }
            else
            {
                Debug.LogWarning("既にリストに含まれています");
            }
        }
        private void OnDisable()
        {
            if (EnemyManager.Instance.EnemyHolder.ActiveEnemyHolder.Contains(this))
            {
                EnemyManager.Instance.EnemyHolder.ActiveEnemyHolder.Remove(this);
            }
            else
            {
                Debug.LogWarning("リストに含まれていません。");
            }
        }
    }
}
