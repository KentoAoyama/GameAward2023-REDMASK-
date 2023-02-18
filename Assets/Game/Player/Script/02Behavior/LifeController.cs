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

        public void Damage(float value)
        {
            if (!_isGodMode)
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
