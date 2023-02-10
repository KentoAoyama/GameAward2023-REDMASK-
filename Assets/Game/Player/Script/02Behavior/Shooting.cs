// 日本語対応
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    /// <summary>
    /// 射撃の基準となるクラス
    /// </summary>
    [System.Serializable]
    public class Shooting
    {
        [Tooltip("射出する弾"), SerializeField]
        private GameObject _bullet = default;
        [Tooltip("発射孔"), SerializeField]
        private Transform _muzzle = default;
        [Tooltip("弾の非接触対象"), SerializeField]
        private Collider2D[] _nonCollisionTarget = default;
        [Tooltip("ゲームパッドで操作するかどうか、この値がfalseであればマウス操作となる"), SerializeField]
        private bool _isGamePadMode = false;
        [Tooltip("照準描画用のラインレンダラーを割り当ててください"), SerializeField]
        private LineRenderer _aimingLineRenderer = null;
        [SerializeField]
        private float _maxLineLength = 1f;

        private PlayerController _playerController = null;
        /// <summary> 撃つ方向 </summary>
        private Vector2 _aimingAngle = Vector2.right;

        public void Init(PlayerController playerController)
        {
            _playerController = playerController;
        }
        public void Update()
        {
            // 撃つ方向を保存する
            if (_isGamePadMode) // ゲームパッド操作の場合
            {
                if ((_playerController.InputManager.GetValue<Vector2>(InputType.LookingAngle)).sqrMagnitude > 0.2f)
                {
                    _aimingAngle = _playerController.InputManager.GetValue<Vector2>(InputType.LookingAngle);
                }
            }
            else // マウス操作中の場合
            {
                var mouseWorldPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
                if ((Vector2)mouseWorldPos - (Vector2)_playerController.transform.position != Vector2.zero)
                {
                    _aimingAngle = mouseWorldPos - _playerController.transform.position;
                }
            }

            if (_playerController.InputManager.IsPressed[InputType.Fire1])
            {
                // マウス操作中の場合
                // ゲームパッド操作の場合
                Shoot();

            } // 攻撃処理
        }

        /// <summary>
        /// 弾を撃つ
        /// </summary>
        private void Shoot()
        {
            // 弾を生成し、弾のセットアップを行う

            if (GameObject.Instantiate(_bullet, _muzzle.position, Quaternion.identity).
                TryGetComponent(out BulletControllerBase bc))
            {
                bc.Setup(_aimingAngle, _nonCollisionTarget);
            }
        }

        /// <summary>
        /// 照準を描画する
        /// </summary>
        public void OnDrawAimingLine()
        {
            // 開始位置を設定
            _aimingLineRenderer.SetPosition(0, _playerController.transform.position);
            // 終了位置を取得/設定
            var endPos = _aimingAngle.normalized * _maxLineLength + (Vector2)_playerController.transform.position;
            _aimingLineRenderer.SetPosition(1, endPos);
        }
    }
}