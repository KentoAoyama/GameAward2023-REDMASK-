using Bullet;
using Cysharp.Threading.Tasks;
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
        private GameObject _bulletPrefab = default;
        [Tooltip("発射孔"), SerializeField]
        private Transform _muzzleTransform = default;
        [Tooltip("弾の発射インターバル"), SerializeField]
        private float _interval = 0.4f;
        [Tooltip("弾の非接触対象"), SerializeField]
        private Collider2D[] _nonCollisionTarget = default;
        [Tooltip("照準描画用のラインレンダラーを割り当ててください"), SerializeField]
        private LineRenderer _aimingLineRenderer = null;
        [Tooltip("ラインの最大長さ"), SerializeField]
        private float _maxLineLength = 1f;

        private PlayerController _playerController = null;
        /// <summary> 撃つ方向 </summary>
        private Vector2 _aimingAngle = Vector2.right;
        /// <summary> 攻撃可能かどうかを表す値 </summary>
        private bool _canFire = true;

        public void Init(PlayerController playerController)
        {
            _playerController = playerController;
        }
        public void Update()
        {
            // 撃つ方向を保存する
            if (_playerController.DeviceManager.CurrentDevice == Input.Device.GamePad) // ゲームパッド操作の場合
            {
                if ((_playerController.InputManager.GetValue<Vector2>(InputType.LookingAngle)).sqrMagnitude > 0.4f)
                {
                    _aimingAngle = _playerController.InputManager.GetValue<Vector2>(InputType.LookingAngle);
                }
            }
            else // マウス操作の場合
            {
                var mouseWorldPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
                if ((Vector2)mouseWorldPos - (Vector2)_playerController.transform.position != Vector2.zero)
                {
                    _aimingAngle = mouseWorldPos - _playerController.transform.position;
                }
            }

            // Debug.Log(_playerController.InputManager.GetValue<float>(InputType.Fire1)); // Rightショルダーの値を確認
            if (_playerController.InputManager.GetValue<float>(InputType.Fire1) > 0.49f &&
                _canFire)
            {
                Shoot();

            } // 攻撃処理
        }
        /// <summary>
        /// 弾を撃つ
        /// </summary>
        private async void Shoot()
        {
            // 弾を生成し、弾のセットアップ処理を実行する
            if (GameObject.Instantiate(_bulletPrefab, _muzzleTransform.position, Quaternion.identity).
                TryGetComponent(out BulletControllerBase bc))
            {
                bc.Setup(_aimingAngle, _nonCollisionTarget);
                _canFire = false;
                // トリガーが持ち上がるまで待機
                await UniTask.WaitUntil(() => _playerController.InputManager.GetValue<float>(InputType.Fire1) < 0.01f);
                // 設定したインターバルを待つ
                await UniTask.Delay((int)(_interval * 1000f));
                _canFire = true;
            }
        }

        /// <summary>
        /// 照準を描画する
        /// </summary>
        public void OnDrawAimingLine()
        {
            // 開始位置を設定
            _aimingLineRenderer.SetPosition(0, _muzzleTransform.position);
            // 終了位置を取得/設定
            var endPos = _aimingAngle
                .normalized * _maxLineLength + (Vector2)_muzzleTransform.position;
            _aimingLineRenderer.SetPosition(1, endPos);
        }
    }
}