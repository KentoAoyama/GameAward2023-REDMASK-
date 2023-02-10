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

        public void Damage(float value)
        {
            _status._life -= value;
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