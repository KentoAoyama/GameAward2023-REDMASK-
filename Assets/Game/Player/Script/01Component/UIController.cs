using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;
using UnityEngine.UI;
using Bullet;

namespace UI
{
    public class UIController : MonoBehaviour
    {
        [SerializeField]
        private PlayerController _playerController = default;
        [SerializeField]
        private BulletSelectUIPresenter _bulletSelectUIPresenter = default;
        [SerializeField]
        private RevolverUIPresenter _revolverUIPresenter = default;
        [SerializeField]
        private HowToPlayUI _howToPlayUI = default;

        public BulletSelectUIPresenter BulletSelectUIPresenter => _bulletSelectUIPresenter;

        private void Start()
        {
            _bulletSelectUIPresenter.Init(_playerController.InputManager, _playerController.BulletCountManager);
            _revolverUIPresenter.Init(_playerController);
            _howToPlayUI.Setup(_playerController.DeviceManager);
        }
        private void Update()
        {
            _bulletSelectUIPresenter.Update();
        }

        [Header("テスト用")]
        [SerializeField]
        private InputField _standardBulletCountInputField = default;
        [SerializeField]
        private InputField _penetrateBulletCountInputField = default;
        [SerializeField]
        private InputField _reflectBulletCountInputField = default;
        /// <summary>
        /// ボタンから呼び出す想定で作成したメソッド。
        /// インプットフィールドに入力された値を所持数に割り当てる。
        /// </summary>
        public void AssignInputFieldValues()
        {
            _playerController.BulletCountManager.SetBullet(BulletType.StandardBullet, StringToInt(_standardBulletCountInputField.text));
            _playerController.BulletCountManager.SetBullet(BulletType.PenetrateBullet, StringToInt(_penetrateBulletCountInputField.text));
            _playerController.BulletCountManager.SetBullet(BulletType.ReflectBullet, StringToInt(_reflectBulletCountInputField.text));
        }
        public int StringToInt(string str) { return string.IsNullOrEmpty(str) ? 0 : int.Parse(str); }
    }
}
