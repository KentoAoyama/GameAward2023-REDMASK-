using Bullet;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;

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
        private Transform[] _muzzleTransform = new Transform[12];
        [Tooltip("弾の発射インターバル"), SerializeField]
        private float _interval = 0.4f;
        [Tooltip("照準描画用のラインレンダラーを割り当ててください"), SerializeField]
        private LineRenderer _aimingLineRenderer = null;
        [Tooltip("ガイドライン用レイヤーマスク"), SerializeField]
        private LayerMask _guidelineLayerMask = default;
        [Tooltip("ガイドライン用レイヤーマスク"), SerializeField]
        private LayerMask _bulletLayerMask = default;
        [TagName, SerializeField]
        private string _gameZone = default;
        // 反射弾用
        [Tooltip("ガイドライン用レイヤーマスク"), SerializeField]
        private LayerMask _guidelineLayerMaskForReflectBullet = default;
        [Tooltip("ガイドライン用レイヤーマスク"), SerializeField]
        private LayerMask _bulletLayerMaskForReflectBullet = default;
        [TagName, SerializeField]
        private string _gameZoneForReflectBullet = default;

        private PlayerController _playerController = null;
        /// <summary> シリンダーを表現する値 </summary>
        private IStoreableInChamber[] _cylinder = new IStoreableInChamber[6];
        /// <summary> トリガーを引いたときに発射するチェンバーの位置を表現する値 </summary>
        private int _currentChamber = 0;
        /// <summary> 撃つ方向 </summary>
        private Vector2 _aimingAngle = Vector2.right;
        /// <summary> 攻撃可能かどうかを表す値 </summary>
        private bool _canFire = true;

        private bool _offLineRendrer = false;

        /// <summary> 現在のチェンバーの位置 </summary>
        public int CurrentChamber => _currentChamber;
        /// <summary> 現在のシリンダーの状態 </summary>
        public IStoreableInChamber[] Cylinder { get => _cylinder; set => _cylinder = value; }
        /// <summary> 発砲可能かどうかを表す値 </summary>
        public bool CanFire => _canFire;

        public event Action<int> OnSetCylinderIndex = default;
        public event Action<int> OnFire = default;
        public Action<int, BulletType> OnChamberStateChanged = default;
        public bool IsPause { get; set; } = false;

        public void Init(PlayerController playerController)
        {
            _playerController = playerController;
        }
        public void Update()
        {
            if (IsPause) return;
            // 撃つ方向を保存する
            if (_playerController.DeviceManager.CurrentDevice.Value == Input.Device.GamePad) // ゲームパッド操作の場合
            {
                if ((_playerController.InputManager.GetValue<Vector2>(InputType.LookingAngleGamePad)).magnitude > 0.5f)
                {
                    _aimingAngle = _playerController.InputManager.GetValue<Vector2>(InputType.LookingAngleGamePad);
                }
            }
            else // マウス操作の場合
            {
                // マウスの座標をワールド座標に変換する
                Vector3 a = _playerController.InputManager.GetValue<Vector2>(InputType.LookingMausePos);
                a.z = 10f;
                var mouseWorldPos = Camera.main.ScreenToWorldPoint(a);
                _aimingAngle = mouseWorldPos - _playerController.BodyAnglSetteing.ArmCenterPos.position; ;
            }
        }

        public void SetCylinderIndex(int index)
        {
            OnSetCylinderIndex?.Invoke(index);
            _currentChamber = index;
        }
        /// <summary> 弾を装填する </summary>
        /// <param name="bullet"> 装填する弾 </param>
        /// <param name="chamberNumber"> 装填するチェンバーの位置 </param>
        /// <returns> 装填に成功したかどうかを表す値 </returns>
        public bool LoadBullet(Bullet2 bullet, int chamberNumber)
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
                    if (bullet == null)
                    {
                        OnChamberStateChanged?.Invoke(chamberNumber, BulletType.Empty);
                    }
                    else
                    {
                        OnChamberStateChanged?.Invoke(chamberNumber, bullet.Type);
                    }

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
            else if (_cylinder[_currentChamber] is Bullet2)
            {
                //音を鳴らす
                if (_playerController.GunSetUp.IsSlowTimeNow)
                {
                    GameManager.Instance.AudioManager.PlaySE("CueSheet_Gun", "SE_Player_Gun_Attack_Slow");
                }
                else
                {
                    GameManager.Instance.AudioManager.PlaySE("CueSheet_Gun", "SE_Player_Attack_Gun");
                }


                var bullet = _cylinder[_currentChamber] as Bullet2;
                // 弾を複製
                var bulletClone = GameObject.Instantiate(bullet.gameObject, _muzzleTransform[_playerController.BodyAnglSetteing.MuzzleNum].position, Quaternion.identity);
                // 弾のコンポーネントを取得
                var bulletComponent = bulletClone.GetComponent<Bullet2>();
                // 移動先のポジションを取得
                var positions = GetPositions2ForBullet2(bulletComponent);

                positions.RemoveAt(0); // 原点を除く
                var positionsClone = new List<Vector2>(positions); // ポジションの集まりを持つリストを複製。（同じインスタンスを利用できない為）
                bulletComponent.SetPositions(positionsClone); // ポジションをセット
                bulletComponent.StartMove(); // 移動を開始

                //弾を撃った場合に、カメラを揺らす
                _playerController.CameraControl.RevolverShootShake();

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
            //構えてないときは描画しない
            if (!_playerController.GunSetUp.IsGunSetUp || _playerController.PlayerAnimatorControl.IsAnimationNow || !_offLineRendrer)
            {
                _aimingLineRenderer.positionCount = 0;
                return;
            }

            // 位置リストを取得
            var potisitons = GetPositions2ForGuideline(_cylinder[_currentChamber] as Bullet2);
            // ラインレンダラーにいくつの位置があるか教える
            _aimingLineRenderer.positionCount = potisitons.Count;
            // ラインレンダラーに各座標を教える
            for (int i = 0; i < potisitons.Count; i++)
            {
                _aimingLineRenderer.SetPosition(i, potisitons[i]);
            }
        }


        /// <summary>リロード中など、照準を消したいときに呼ぶ</summary>
        public void OffDrawAimingLine(bool isOn)
        {
            _offLineRendrer = isOn;
        }



        /// <summary> ガイドライン用のポジションをまとめて持つリスト </summary>
        private List<Vector2> _potisions = new List<Vector2>();
        /// <summary> ガイドライン用のポジションをまとめて持つリスト </summary>
        private List<Vector2> _potisions2 = new List<Vector2>();
        /// <summary> Bullet2のガイドライン用 </summary>
        /// <param name="bullet"></param>
        /// <returns></returns>
        private List<Vector2> GetPositions2ForGuideline(Bullet2 bullet)
        {
            _potisions.Clear(); // リストをクリア
            _potisions.Add(_muzzleTransform[_playerController.BodyAnglSetteing.MuzzleNum].position); // 原点を追加

            if (bullet == null) return _potisions;

            switch (bullet.Type)
            {
                case BulletType.StandardBullet: // 標準弾のガイドラインポジションをリストに追加
                    // 原点から指定の方向にレイを飛ばす。
                    var hitStd = Physics2D.Raycast(_muzzleTransform[_playerController.BodyAnglSetteing.MuzzleNum].position, _aimingAngle, bullet.GuidelineLength, _guidelineLayerMask);
                    // レイがヒットしたらその位置をリストに追加
                    if (hitStd.collider != null) _potisions.Add(hitStd.point);
                    // ヒットしなかったらレイの先端位置をリストに追加
                    else _potisions.Add(_muzzleTransform[_playerController.BodyAnglSetteing.MuzzleNum].position + (Vector3)_aimingAngle.normalized * bullet.GuidelineLength);
                    break;
                case BulletType.PenetrateBullet: // 貫通弾のガイドラインポジションをリストに追加
                    // レイを飛ばす
                    var hitsPen = Physics2D.RaycastAll(_muzzleTransform[_playerController.BodyAnglSetteing.MuzzleNum].position, _aimingAngle, bullet.GuidelineLength, _guidelineLayerMask);
                    var penetrate = bullet as PenetrateBullet2;
                    var lengthPen = bullet.GuidelineLength; // 半端分用の値
                    bool _isHitGameZone = false;
                    // 何かにヒットしていれば貫通して最終的に届く位置まで取得する
                    for (int i = 0; i < hitsPen.Length && i < penetrate.MaxWallHitNumber + 1; i++)
                    {
                        _potisions.Add(hitsPen[i].point);
                        lengthPen -= (_potisions[_potisions.Count - 1] - _potisions[_potisions.Count - 2]).magnitude;

                        if (_isHitGameZone = hitsPen[i].collider.tag == _gameZone)
                        {
                            break;
                        }
                    }
                    // 半端分が余っているときの処理。
                    if (hitsPen.Length <= penetrate.MaxWallHitNumber && !_isHitGameZone)
                    {
                        _potisions.Add(_potisions[_potisions.Count - 1] + _aimingAngle.normalized * lengthPen);
                    }
                    break;
                case BulletType.ReflectBullet: // 反射弾のガイドラインポジションをリストに追加
                    var reflect = bullet as ReflectBullet2; // 本来の型に変換
                    var length = reflect.GuidelineLength;  // 残りの長さ
                    Vector2 pos = _muzzleTransform[_playerController.BodyAnglSetteing.MuzzleNum].position; // 位置を保存する用の変数。（初期位置は、マズルの位置）
                    Vector2 dir = _aimingAngle; // 最初の角度ベクトル。（初期位置は、プレイヤーからマウス座標へのベクトルか右スティックの角度）
                    RaycastHit2D hit; // レイのヒット情報

                    for (int i = 0; i < reflect.MaxWallCollisionCount; i++)
                    {
                        // 位置の計算についてメモ(レイが対象に埋まるのでちょっとずらす)
                        hit = Physics2D.Raycast(pos + dir * 0.01f, dir, length, _guidelineLayerMaskForReflectBullet);

                        if (hit.collider != null) // レイが当たった時の処理
                        {
                            _potisions.Add(hit.point);                         // 当たった位置をリストに追加
                            length -= (pos - hit.point).magnitude;             // 長さを減算
                            pos = hit.point;                                   // 位置を更新
                            dir = Vector2.Reflect(dir, hit.normal.normalized); // 角度を反転

                            if (hit.collider.tag == _gameZoneForReflectBullet)
                            {
                                break;
                            }
                        }
                        else // レイが当たらなかった時の処理
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
        /// <summary> 
        /// Bullet2の実際の弾用 <br/>
        /// ガイドライン用と異なる点 <br/>
        /// GuideLineLengthに関係なくどこまでも飛ぶ。
        /// </summary>
        /// <param name="bullet"></param>
        /// <returns></returns>
        private List<Vector2> GetPositions2ForBullet2(Bullet2 bullet)
        {
            _potisions2.Clear(); // リストをクリア
            _potisions2.Add(_muzzleTransform[_playerController.BodyAnglSetteing.MuzzleNum].position); // 原点を追加

            if (bullet == null) return _potisions2;

            switch (bullet.Type)
            {
                case BulletType.StandardBullet: // 標準弾のガイドラインポジションをリストに追加
                    // 原点から指定の方向にレイを飛ばす。
                    var hitStd = Physics2D.Raycast(_muzzleTransform[_playerController.BodyAnglSetteing.MuzzleNum].position, _aimingAngle, 1000f, _bulletLayerMask);
                    // レイがヒットしたらその位置をリストに追加
                    if (hitStd.collider != null) _potisions2.Add(hitStd.point);
                    // ヒットしなかったらレイの先端位置をリストに追加
                    else _potisions2.Add(_muzzleTransform[_playerController.BodyAnglSetteing.MuzzleNum].position + (Vector3)_aimingAngle.normalized * 1000f);
                    break;
                case BulletType.PenetrateBullet: // 貫通弾のガイドラインポジションをリストに追加
                    // レイを飛ばす
                    var hitsPen = Physics2D.RaycastAll(_muzzleTransform[_playerController.BodyAnglSetteing.MuzzleNum].position, _aimingAngle, 1000f, _bulletLayerMask);
                    var penetrate = bullet as PenetrateBullet2;
                    bool _isHitGameZone = false;
                    // 何かにヒットしていれば貫通して最終的に届く位置まで取得する
                    for (int i = 0; i < hitsPen.Length && i < penetrate.MaxWallHitNumber + 1; i++)
                    {
                        _potisions2.Add(hitsPen[i].point);
                        if (_isHitGameZone = hitsPen[i].collider.tag == _gameZone)
                        {
                            break;
                        }
                    }
                    // 半端分が余っているときの処理。
                    if (hitsPen.Length <= penetrate.MaxWallHitNumber && !_isHitGameZone)
                    {
                        _potisions2.Add(_potisions2[_potisions2.Count - 1] + _aimingAngle.normalized * 1000f);
                    }
                    break;
                case BulletType.ReflectBullet: // 反射弾のガイドラインポジションをリストに追加
                    var reflect = bullet as ReflectBullet2; // 本来の型に変換
                    Vector2 pos = _muzzleTransform[_playerController.BodyAnglSetteing.MuzzleNum].position; // 位置を保存する用の変数。（初期位置は、マズルの位置）
                    Vector2 dir = _aimingAngle; // 最初の角度ベクトル。（初期位置は、プレイヤーからマウス座標へのベクトルか右スティックの角度）
                    RaycastHit2D hit; // レイのヒット情報

                    for (int i = 0; i < reflect.MaxWallCollisionCount; i++)
                    {
                        // 位置の計算についてメモ(レイが対象に埋まるのでちょっとずらす)
                        hit = Physics2D.Raycast(pos + dir * 0.01f, dir, 1000f, _bulletLayerMaskForReflectBullet);

                        if (hit.collider != null) // レイが当たった時の処理
                        {
                            _potisions2.Add(hit.point);                         // 当たった位置をリストに追加
                            pos = hit.point;                                   // 位置を更新
                            dir = Vector2.Reflect(dir, hit.normal.normalized); // 角度を反転

                            if (hit.collider.tag == _gameZoneForReflectBullet)
                            {
                                break;
                            }
                        }
                        else // レイが当たらなかった時の処理
                        {
                            _potisions2.Add(pos + dir.normalized * 1000f);
                            break;
                        }
                    }
                    break;
                default: return _potisions2;
            }
            return _potisions2;
        }
    }
}