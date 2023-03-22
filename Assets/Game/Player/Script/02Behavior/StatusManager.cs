using System;
using UnityEngine;

namespace Player
{
    /// <summary>
    /// ステータス管理用クラス
    /// </summary>
    [System.Serializable]
    public class StatusManager : IDamageable
    {
        [SerializeField]
        private PlayerStatus _status;

        public PlayerStatus Status => _status;

        public void Damage()
        {

        }
    }
}
/// <summary>
/// プレイヤーのステータスを表す構造体
/// </summary>
[System.Serializable]
public struct PlayerStatus
{
    public float _life;
}