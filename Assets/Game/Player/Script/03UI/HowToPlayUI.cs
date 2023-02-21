// 日本語対応
using UnityEngine;
using System;
using UnityEngine.UI;
using Input;
using UniRx;

namespace Player
{
    /// <summary>
    /// 遊びかたを描画するクラス
    /// </summary>
    [Serializable]
    public class HowToPlayUI
    {
        [SerializeField]
        private Image _moveImage = default;
        [SerializeField]
        private Image _jumpImage = default;
        [SerializeField]
        private Image _ballisticsImage = default;
        [SerializeField]
        private Image _fireImage = default;
        [SerializeField]
        private Image _avoidanceImage = default;
        [SerializeField]
        private Image _bulletSelectImage = default;
        [SerializeField]
        private Image _loadImage = default;


        [SerializeField]
        private HowToPlaySprites _keyboardAndMouseSprites = default;
        [SerializeField]
        private HowToPlaySprites _gamepadSprites = default;

        public void Setup(DeviceManager deviceManager)
        {
            deviceManager.CurrentDevice.Subscribe(device => Assign(device));
        }
        private void Assign(Device device)
        {
            if (device == Device.KeyboardAndMouse)
            {
                SetSprites(_keyboardAndMouseSprites);
            }
            else if (device == Device.GamePad)
            {
                SetSprites(_gamepadSprites);
            }
        }

        private void SetSprites(HowToPlaySprites howToPlaySprites)
        {
            _moveImage.sprite = howToPlaySprites._move;
            _jumpImage.sprite = howToPlaySprites._jump;
            _ballisticsImage.sprite = howToPlaySprites._ballistics;
            _fireImage.sprite = howToPlaySprites._fire;
            _avoidanceImage.sprite = howToPlaySprites._avoidance;
            _bulletSelectImage.sprite = howToPlaySprites._bulletSelect;
            _loadImage.sprite = howToPlaySprites._load;
        }


        [Serializable]
        public struct HowToPlaySprites
        {
            public Sprite _move;
            public Sprite _jump;
            public Sprite _ballistics;
            public Sprite _fire;
            public Sprite _avoidance;
            public Sprite _bulletSelect;
            public Sprite _load;
        }
    }
}
