using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

namespace UI
{
    public class UIController : MonoBehaviour
    {
        [SerializeField]
        private PlayerController _playerController = default;
        [SerializeField]
        private BulletSelectUIPresenter _bulletSelectUIPresenter = default;

        private void Start()
        {
            _bulletSelectUIPresenter.Init(_playerController.InputManager, _playerController.BulletsManager);
        }

        private void Update()
        {
            _bulletSelectUIPresenter.Update();
        }
    }
}
