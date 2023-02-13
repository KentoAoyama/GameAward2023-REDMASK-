using Bullet;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    /// <summary>
    /// リボルバーを表現するクラス <br/>
    /// 参考資料 :
    /// https://hb-plaza.com/parts/#:~:text=hb%2Dplaza.com-,%E3%83%AA%E3%83%9C%E3%83%AB%E3%83%90%E3%83%BC%EF%BC%88%E3%82%B9%E3%82%BF%E3%83%BC%E3%83%A0%E3%83%AB%E3%82%AC%E3%83%BCGP100%EF%BC%89,-1
    /// </summary>
    [System.Serializable]
    public class Revolver
    {
        [Tooltip("殻薬莢"), SerializeField]
        private ShellCase _shellCase = default;
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
        /// <summary> シリンダーを表現する値 </summary>
        private BulletBase[] _cylinder = new BulletBase[6];
        /// <summary> トリガーを引いたときに発射するチェンバーの位置を表現する値 </summary>
        private int _currentChamber = 0;
        /// <summary> 撃つ方向 </summary>
        private Vector2 _aimingAngle = Vector2.right;
        /// <summary> 攻撃可能かどうかを表す値 </summary>
        private bool _canFire = true;

        /// <summary> 現在のシリンダーの状態 </summary>
        public BulletBase[] Cylinder => _cylinder;

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

            // 撃てる状態かつ、撃つ入力が発生したとき 銃を撃つ
            if (_playerController.InputManager.GetValue<float>(InputType.Fire1) > 0.49f &&
                _canFire)
            {
                Fire();
            }
        }

        /// <summary> 弾を装填する </summary>
        /// <param name="bullet"> 装填する弾 </param>
        /// <param name="chamberNumber"> 装填するチェンバーの位置 </param>
        /// <returns> 装填に成功したかどうかを表す値 </returns>
        public bool LoadBullet(BulletBase bullet, int chamberNumber)
        {
            try
            {
                // 弾か殻薬莢が入っていたら装填できない
                if (_cylinder[chamberNumber] != null)
                {
                    // 弾か殻薬莢が入っている時の処理
                    return false;
                }
                else
                {
                    _cylinder[chamberNumber] = bullet;
                    return true;
                }
            }
            catch (IndexOutOfRangeException e)
            {
                Debug.LogError(
                    $"範囲外の値が指定されました！修正してください！\n" +
                    $"指定された値 :{chamberNumber}");
                Debug.LogError(e.Message);
                return false;
            }
        }
        /// <summary> 一つのチェンバーの 弾, 殻薬莢を排出する。 </summary>
        /// /// <param name="chamberNumber"> 排出するチェンバーの位置 </param>
        public BulletBase[] EjectBulletsAndShells(int chamberNumber)
        {
            try
            {
                _cylinder[chamberNumber] = null;
            }
            catch (IndexOutOfRangeException e)
            {
                Debug.LogError(
                    $"範囲外の値が指定されました！修正してください！\n" +
                    $"指定された値 :{chamberNumber}");
                Debug.LogError(e.Message);
            }
            return _cylinder;
        }
        /// <summary> 全ての殻薬莢を排出する。 </summary>
        public BulletBase[] EjectShellsAll()
        {
            for (int i = 0; i < _cylinder.Length; i++)
            {
                if (_cylinder[i] is IShellCase)
                {
                    _cylinder[i] = null;
                }
            }
            return _cylinder;
        }
        /// <summary> 全てのチェンバーの 弾, 殻薬莢を排出する。 </summary>
        /// <returns> 排出後のシリンダーの状態を返す。 </returns>
        public BulletBase[] EjectBulletsAndShellsAll()
        {
            for (int i = 0; i < _cylinder.Length; i++)
            {
                _cylinder[i] = null;
            }
            return _cylinder;
        }
        /// <summary> 発砲する </summary>
        private async void Fire()
        {
            // 弾であれば発射する。その後、殻薬莢が残る。
            if (_cylinder[_currentChamber] is IBullet)
            {
                // 弾を生成し、弾のセットアップ処理を実行する
                if (GameObject.Instantiate(_cylinder[_currentChamber].gameObject, _muzzleTransform.position, Quaternion.identity).
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

                _cylinder[_currentChamber] = _shellCase; // 殻薬莢を残す
            }

            // チェンバーの位置が変わる（シリンダーが回転する。）
            _currentChamber++;
            _currentChamber %= _cylinder.Length;
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