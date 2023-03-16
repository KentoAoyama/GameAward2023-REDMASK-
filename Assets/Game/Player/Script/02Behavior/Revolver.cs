using Bullet;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
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
        [Tooltip("照準描画用のラインレンダラーを割り当ててください"), SerializeField]
        private LineRenderer _aimingLineRenderer = null;
        [Tooltip("ガイドライン用レイヤーマスク"), SerializeField]
        private LayerMask _guidelineLayerMask = default;

        private PlayerController _playerController = null;
        /// <summary> シリンダーを表現する値 </summary>
        private IStoreableInChamber[] _cylinder = new IStoreableInChamber[6];
        /// <summary> トリガーを引いたときに発射するチェンバーの位置を表現する値 </summary>
        private int _currentChamber = 0;
        /// <summary> 撃つ方向 </summary>
        private Vector2 _aimingAngle = Vector2.right;
        /// <summary> 攻撃可能かどうかを表す値 </summary>
        private bool _canFire = true;

        /// <summary> 現在のチェンバーの位置 </summary>
        public int CurrentChamber => _currentChamber;
        /// <summary> 現在のシリンダーの状態 </summary>
        public IStoreableInChamber[] Cylinder => _cylinder;
        /// <summary> 発砲可能かどうかを表す値 </summary>
        public bool CanFire => _canFire;

        public event Action<int> OnFire = default;
        public Action<int, BulletType> OnChamberStateChanged = default;

        public void Init(PlayerController playerController)
        {
            _playerController = playerController;
        }
        public void Update()
        {
            // 撃つ方向を保存する
            if (_playerController.DeviceManager.CurrentDevice.Value == Input.Device.GamePad) // ゲームパッド操作の場合
            {
                if ((_playerController.InputManager.GetValue<Vector2>(InputType.LookingAngle)).sqrMagnitude > 0.5f)
                {
                    _aimingAngle = _playerController.InputManager.GetValue<Vector2>(InputType.LookingAngle);
                }
            }
            else // マウス操作の場合
            {
                var mouseWorldPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
                if (((Vector2)mouseWorldPos - (Vector2)_playerController.transform.position).sqrMagnitude > 0.5f)
                {
                    _aimingAngle = mouseWorldPos - _playerController.transform.position;
                }
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
                    Debug.LogWarning("そこには弾か殻薬莢が入っています");
                    return false;
                }
                else
                {
                    OnChamberStateChanged?.Invoke(chamberNumber, bullet.Type);
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
        /// <returns> 排出後のシリンダーの状態を返す。 </returns>
        public IStoreableInChamber[] EjectBulletsAndShells(int chamberNumber)
        {
            try
            {
                _cylinder[chamberNumber] = null;
                OnChamberStateChanged?.Invoke(chamberNumber, BulletType.Empty);
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
        /// <returns> 排出後のシリンダーの状態を返す。 </returns>
        public IStoreableInChamber[] EjectShellsAll()
        {
            for (int i = 0; i < _cylinder.Length; i++)
            {
                if (_cylinder[i] is ShellCase)
                {
                    _cylinder[i] = null;
                    OnChamberStateChanged?.Invoke(i, BulletType.Empty);
                }
            }
            return _cylinder;
        }
        /// <summary> 全てのチェンバーの 弾, 殻薬莢を排出する。 </summary>
        /// <returns> 排出後のシリンダーの状態を返す。 </returns>
        public IStoreableInChamber[] EjectBulletsAndShellsAll()
        {
            for (int i = 0; i < _cylinder.Length; i++)
            {
                _cylinder[i] = null;
                OnChamberStateChanged?.Invoke(i, BulletType.Empty);
            }
            return _cylinder;
        }
        /// <summary> 発砲する </summary>
        public async void Fire()
        {
            if (_playerController.DeviceManager.CurrentDevice.Value == Input.Device.Switching)
            {
                _canFire = false;
                await UniTask.WaitUntil(() => _playerController.InputManager.GetValue<float>(InputType.Fire1) < 0.01f);
                _canFire = true;
                return;
            }
            // 弾であれば発射する。その後、殻薬莢が残る。
            else if (_cylinder[_currentChamber] is BulletBase)
            {
                var bullet = _cylinder[_currentChamber] as BulletBase;
                // 弾を複製し、弾のセットアップ処理を実行する
                var bulletClone = GameObject.Instantiate(bullet.gameObject, _muzzleTransform.position, Quaternion.identity);
                var b = bulletClone.GetComponent<BulletBase>();
                b.Setup(_aimingAngle);
                if (b is ReflectBullet)
                {
                    (b as ReflectBullet).ToStart(GetPositions(b).ToArray());
                }

                _cylinder[_currentChamber] = _shellCase; // 殻薬莢を残す
                OnChamberStateChanged?.Invoke(_currentChamber, BulletType.ShellCase);
            }

            // チェンバーの位置が変わる（シリンダーが回転する。）
            _currentChamber++;
            _currentChamber %= _cylinder.Length;

            OnFire?.Invoke(_currentChamber);

            // インターバル系処理
            _canFire = false;
            // トリガーが持ち上がるまで待機
            await UniTask.WaitUntil(() => _playerController.InputManager.GetValue<float>(InputType.Fire1) < 0.01f);
            // 設定したインターバルを待つ
            await UniTask.Delay((int)(_interval * 1000f));
            _canFire = true;
        }
        /// <summary>
        /// 照準を描画する
        /// </summary>
        public void OnDrawAimingLine()
        {
            var potisitons = GetPositions(_cylinder[_currentChamber] as BulletBase);
            _aimingLineRenderer.positionCount = potisitons.Count;

            for (int i = 0; i < potisitons.Count; i++)
            {
                _aimingLineRenderer.SetPosition(i, potisitons[i]);
            }
        }

        private List<Vector2> _potisions = new List<Vector2>();
        private List<Vector2> GetPositions(BulletBase bullet)
        {
            _potisions.Clear();
            _potisions.Add(_muzzleTransform.position);

            if (bullet == null) return _potisions;

            switch (bullet.Type)
            {
                case BulletType.StandardBullet: // 標準弾のガイドラインポジションをリストに追加
                    var hitStd = Physics2D.Raycast(_muzzleTransform.position, _aimingAngle, bullet.GuidelineLength, _guidelineLayerMask);
                    if (hitStd.collider != null) _potisions.Add(hitStd.point);
                    else _potisions.Add(_muzzleTransform.position + (Vector3)_aimingAngle.normalized * bullet.GuidelineLength);
                    break;
                case BulletType.PenetrateBullet: // 貫通弾のガイドラインポジションをリストに追加
                    var hitsPen = Physics2D.RaycastAll(_muzzleTransform.position, _aimingAngle, bullet.GuidelineLength, _guidelineLayerMask);
                    var penetrate = bullet as PenetrateBullet;
                    for (int i = 0; i < hitsPen.Length && i < penetrate.MaxWallHitNumber + 1; i++)
                    {
                        _potisions.Add(hitsPen[i].point);
                    }
                    if (_potisions.Count == 1)
                    {
                        _potisions.Add(_muzzleTransform.position + (Vector3)_aimingAngle.normalized * bullet.GuidelineLength);
                    }
                    break;
                case BulletType.ReflectBullet: // 反射弾のガイドラインポジションをリストに追加
                    var reflect = bullet as ReflectBullet;

                    var length = reflect.GuidelineLength;
                    Vector2 pos = _muzzleTransform.position;
                    Vector2 dir = _aimingAngle;

                    RaycastHit2D hit;

                    for (int i = 0; i < reflect.MaxWallCollisionCount; i++)
                    {
                        // 位置の計算についてメモ(レイが対象に埋まるのでちょっとずらす)
                        hit = Physics2D.Raycast(pos + dir * 0.1f, dir, length, _guidelineLayerMask);

                        if (hit.collider != null)
                        {
                            _potisions.Add(hit.point);                         // 当たった位置をリストに追加
                            length -= (pos - hit.point).magnitude;             // 長さを減算
                            pos = hit.point;                                   // 位置を更新
                            dir = Vector2.Reflect(dir, hit.normal.normalized); // 角度を反転
                        }
                        else
                        {
                            _potisions.Add(pos + dir.normalized * length);
                            break;
                        }
                    }
                    break;
                default: return _potisions;
            }

            return _potisions;
        }
    }
}