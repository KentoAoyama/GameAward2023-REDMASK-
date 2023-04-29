// 日本語対応
using System;
using UnityEngine;

namespace Player
{
    [Serializable]
    public class LifeController
    {
        [SerializeField]
        private float _life = 100f;
        [SerializeField]
        private bool _isGodMode = false;

        public event Action OnDeath = default;
        public bool IsGodMode { get => _isGodMode; set => _isGodMode = value; }

        private bool _isAvoid = false;

        public bool IsAvoid { get => _isAvoid; set => _isAvoid = value; }

        public void Damage(float value)
        {
            if (!_isGodMode && !_isAvoid)
            {
                _life -= value;
                if (_life < 1)
                {
                    Debug.LogError("ライフがなくなりました");
                    OnDeath?.Invoke();
                }
            }
        }
    }
}
