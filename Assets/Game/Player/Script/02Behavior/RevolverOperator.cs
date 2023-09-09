
using Bullet;
using UI;
using UnityEngine;
using System;


namespace Player
{
    /// <summary>
    /// リボルバー操作クラス
    /// </summary>
    [System.Serializable]
    public class RevolverOperator
    {
        private PlayerController _playerController = null;

        [Header("Test用。排莢を分かりやすくするためのText")]
        [SerializeField] private GameObject _excretedText;

        [Header("Test用。リロード中を分かりやすくするためのText")]
        [SerializeField] private GameObject _setBulletText;

        [Header("排莢にかかる時間")]
        [SerializeField] private float _excretedPodsTime = 1;

        [Header("弾を込めるのにかかる時間")]
        [SerializeField] private float _setBulletTime = 1;

        [Header("シリンダーの回転を変えるのにかかる時間")]
        [SerializeField] private float _changeChamberTime = 1;

        [Header("マズルフラッシュ")]
        [SerializeField] private MuzzleFlashController _muzzleFlash;

        [SerializeField] private UIController _uIController;

        [SerializeField] private CartridgeController _cartridgeController;

        /// <summary>空の弾を取り出す時間を計測する</summary>
        private float _countExcretedPodsTime = 0;

        /// <summary>弾を籠める時間を計測する</summary>
        private float _countSetBulletTime = 0;

        private float _chanberChangeNum = 0;

        private float _countChangeChamberTime = 0;

        private bool _isExcretedPods = false;


        private bool _isSetBullet = false;

        /// <summary>現在発砲中かどうか</summary>
        private bool _isFireNow = false;

        /// <summary>現在、リロード中かどうか</summary>
        private bool _isReLoadNow = false;

        /// <summary>シリンダーを開いているかどうか</summary>
        private bool _isOpenCilinder = false;

        /// <summary>チェンバーを回転中かどうか</summary>
        private bool _isChangeCillinderPos;

        /// <summary>現在リロード等によって銃を構えていないかどうか</summary>
        private bool _isNoneSetUp = false;


        public bool IsNoneSetUp => _isNoneSetUp;
        
        public bool IsFireNow { get => _isFireNow; set => _isFireNow = value; }

        public void Init(PlayerController playerController)
        {
            _playerController = playerController;
        }
        public void Update()
        {
            if (GameManager.Instance.PauseManager.PauseCounter > 0)
            {
                return;
            } // ポーズ中は何もできない

            if (_playerController.Proximity.IsProximityNow || !_playerController.GroungChecker.IsHit(_playerController.DirectionControler.MovementDirectionX))
            {
                return;
            }//近接攻撃中はできない

            //構えているとき
            if (_playerController.GunSetUp.IsGunSetUp)
            {
                //撃てる球があるとき
                if (CheckBullet(BulletType.StandardBullet) > 0 || CheckBullet(BulletType.PenetrateBullet) > 0 || CheckBullet(BulletType.ReflectBullet) > 0)
                {
                    // 撃てる状態かつ、撃つ入力が発生したとき 銃を撃つ
                    if (_playerController.InputManager.GetValue<float>(InputType.Fire1) > 0.49f &&
                    _playerController.Revolver.CanFire && !_playerController.PlayerAnimatorControl.IsAnimationNow)
                    {
                        //打てる球があるかどうかの確認
                        int count = 0;

                        foreach (var cylinder in _playerController.Revolver.Cylinder)
                        {
                            if (cylinder == null)
                            {
                                continue;
                            }

                            if (cylinder.Type != BulletType.ShellCase)
                            {
                                count++;
                            }
                        }

                        if (count == 0) return;


                        //アニメーションの再生
                        if (_playerController.Avoidance.IsAvoidanceNow) _playerController.PlayerAnimatorControl.PlayAnimation(PlayerAnimationControl.AnimaKind.AvoidFire);
                        else _playerController.PlayerAnimatorControl.PlayAnimation(PlayerAnimationControl.AnimaKind.Fire);

                        //敵にプレイヤーの位置を通達
                        ReactionMessageSender.SendMessage(_playerController.Player.transform);

                        _isFireNow = true;

                        //マズルフラッシュを再生
                        _muzzleFlash.PlayMuzzleFlash();

                        //発砲処理
                        _playerController.Revolver.Fire();

                        //リロードの中断処理
                        StopRevolverReLoad(false);

                        return;
                    }
                }
            }

            if (_playerController.Avoidance.IsAvoidanceNow || _isFireNow)
            {
                return;
            } //回避中はできない

            int numBulletInRevolver = CheckBullet(BulletType.PenetrateBullet) + CheckBullet(BulletType.ReflectBullet) + CheckBullet(BulletType.StandardBullet + CheckBullet(BulletType.ShellCase));

            int nowSelectBulletNum = SelectBulletPossession(_uIController.BulletSelectUIPresenter.CurrentSelectBulletType.Value);



            //薬室が、空か空薬莢のある場合のみリロード処理をする

            if (_playerController.InputManager.IsPressed[InputType.ChangeSilinder])
            {
                //排出、弾を籠めている最中に押して何度も呼ばれないようにする
                if (_isExcretedPods || _isChangeCillinderPos || _isSetBullet) return;

                if(!_isOpenCilinder)
                {
                    _isExcretedPods = true;
                }

                _isNoneSetUp = true;

                _chanberChangeNum = _playerController.InputManager.GetValue<float>(InputType.ChangeSilinder) > 0f ? 1f : -1f;

                _isChangeCillinderPos = true;

                _playerController.Revolver.OffDrawAimingLine(false);

                _isReLoadNow = true;

                //特定行動中に構えを解除していないかどうかを確認する
                _playerController.GunSetUp.CanselSetUpping();



            }
            else if (CheckBullet(BulletType.ShellCase) > 0 || (numBulletInRevolver != 6 && nowSelectBulletNum > 0))
            {
                if (_playerController.InputManager.IsPressed[InputType.LoadBullet])
                {
                    //排出、弾を籠めている最中に押して何度も呼ばれないようにする
                    if (_isExcretedPods || _isSetBullet) return;

                    _playerController.Revolver.OffDrawAimingLine(false);

                    _isReLoadNow = true;

                    _isNoneSetUp = true;

                    //特定行動中に構えを解除していないかどうかを確認する
                    _playerController.GunSetUp.CanselSetUpping();

                    bool isShallCase = false;

                    foreach (var cylinder in _playerController.Revolver.Cylinder)
                    {
                        if (cylinder == null)
                        {
                            continue;
                        }

                        if (cylinder.Type == BulletType.ShellCase)
                        {
                            isShallCase = true;
                        }

                    }


                    //空の薬莢が残っていたら排出
                    if (isShallCase)
                    {
                        _isExcretedPods = true;
                    }
                    else
                    {
                        // 空いているチャンバーを検索する。
                        var index = FindEmptyChamber();

                        if (index != -1) // 空いているチャンバーが見つかった場合の処理
                        {
                            _isSetBullet = true;
                        }
                        else
                        {
                            return;
                        }  //チェンバーが空いていないので何もしない
                    }
                }
            }




            //排莢の処理
            if (_isExcretedPods)
            {
                /////////////////////////////////TEST用!!!!!!!!!!!!!!!!//////////////////////////
                _excretedText.SetActive(true);

                //排莢のアニメーション
                _playerController.PlayerAnimatorControl.PlayAnimation(PlayerAnimationControl.AnimaKind.ReLoadStart);

                _countExcretedPodsTime += Time.deltaTime;
                if (_excretedPodsTime < _countExcretedPodsTime)
                {
                    /////////////////////////////////TEST用!!!!!!!!!!!!!!!!//////////////////////////
                    _excretedText.SetActive(false);

                    _isOpenCilinder = true;
                    _isExcretedPods = false;

                    if (!_isChangeCillinderPos)
                    {
                        //薬莢排出の処理
                        _cartridgeController.CartridgePlay(CheckBullet(BulletType.ShellCase));

                        // 排莢する
                        var cylinder = _playerController.Revolver.EjectShellsAll();
                        _countExcretedPodsTime = 0;




                        // 空いているチャンバーを検索する。
                        var index = FindEmptyChamber();
                        if (index != -1) // 空いているチャンバーが見つかった場合の処理
                        {
                            //弾を持っていたら弾籠めに以降
                            if (nowSelectBulletNum > 0)
                                _isSetBullet = true;
                        }
                    }
                }
            }
            else if (_isChangeCillinderPos)
            {

                _countChangeChamberTime += Time.deltaTime;

                if (_changeChamberTime < _countChangeChamberTime)
                {
                    //弾を籠めるアニメーション
                    _playerController.PlayerAnimatorControl.PlayAnimation(PlayerAnimationControl.AnimaKind.ReLoad);

                    _playerController.Revolver.ChangeChamber(_chanberChangeNum);
                    _isChangeCillinderPos = false;
                    _countChangeChamberTime = 0;
                }

            }
            else if (_isSetBullet)   //弾を籠める処理
            {
                /////////////////////////////////TEST用!!!!!!!!!!!!!!!!//////////////////////////
                _setBulletText.SetActive(true);

                _countSetBulletTime += Time.deltaTime;
                if (_setBulletTime < _countSetBulletTime)
                {
                    /////////////////////////////////TEST用!!!!!!!!!!!!!!!!//////////////////////////
                    _setBulletText.SetActive(false);

                    //弾を籠めるアニメーション
                    _playerController.PlayerAnimatorControl.PlayAnimation(PlayerAnimationControl.AnimaKind.ReLoad);
                    Debug.Log("N");

                    //特定行動中に構えを解除していないかどうかを確認する
                    //_playerController.GunSetUp.AnimEndSetUpCheck();

                    //音を鳴らす
                    GameManager.Instance.AudioManager.PlaySE("CueSheet_Gun", "SE_Player_Reroad");

                    // 空いているチャンバーを検索する。
                    var index = FindEmptyChamber();


                    //最大まで弾を居れたら強制的に構えに戻す
                    if (index == 5 && _playerController.GunSetUp.IsGunSetUp)
                    {
                        //  _playerController.PlayerAnimatorControl.GunSet(false);
                    }

                    // UIで現在選択している弾を装填する
                    if (_playerController.BulletDataBase.Bullets.TryGetValue(
                            _playerController.UIController.BulletSelectUIPresenter.CurrentSelectBulletType.Value,
                            out Bullet2 bullet))
                    {
                        // 弾を減らす。弾を減らせなかった場合、処理しない。
                        if (_playerController.BulletCountManager.ReduceOneBullet(_playerController.UIController.BulletSelectUIPresenter.CurrentSelectBulletType.Value))
                        {
                            _playerController.Revolver.Cylinder[index] = bullet;
                            _playerController.Revolver.OnChamberStateChanged
                                (index, _playerController.UIController.BulletSelectUIPresenter.CurrentSelectBulletType.Value);
                        }
                    }
                    else // 弾の取得に失敗した場合の処理
                    {
                        Debug.LogError(
                            $"装填に失敗しました。\n" +
                            $"_playerController.BulletDataBase.Bulletsに" +
                            $"{_playerController.UIController.BulletSelectUIPresenter.CurrentSelectBulletType}" +
                            $"は登録されていません！修正してください！");
                    }

                    _isSetBullet = false;
                    _countSetBulletTime = 0;
                }

            }
        }


        public int SelectBulletPossession(BulletType bulletType)
        {
            if (bulletType == BulletType.StandardBullet)
            {
                return _playerController.BulletCountManager.StandardBulletCount.Value;
            }
            else if (bulletType == BulletType.ReflectBullet)
            {
                return _playerController.BulletCountManager.ReflectBulletCount.Value;
            }
            else if (bulletType == BulletType.PenetrateBullet)
            {
                return _playerController.BulletCountManager.PenetrateBulletCount.Value;
            }
            else
            {
                return -1;
            }
        }


        /// <summary>引数で与えた弾が何発あるかを確認する</summary>
        /// <param name="bulletType"></param>
        /// <returns></returns>
        public int CheckBullet(BulletType bulletType)
        {
            //打てる球があるかどうかの確認
            int count = 0;

            foreach (var cylinder in _playerController.Revolver.Cylinder)
            {
                if (cylinder == null)
                {
                    continue;
                }

                if (cylinder.Type == bulletType)
                {
                    count++;
                }
            }

            return count;
        }



        /// <summary>他の行動をしたことによるリロードの中断</summary>
        public void StopRevolverReLoad(bool isAnim)
        {
            /////////////////////////////////TEST用!!!!!!!!!!!!!!!!//////////////////////////
            _excretedText.SetActive(false);
            _setBulletText.SetActive(false);

            _isNoneSetUp = false;

            if (_isReLoadNow && isAnim)
            {
                //弾を籠めるアニメーション
                _playerController.PlayerAnimatorControl.PlayAnimation(PlayerAnimationControl.AnimaKind.ReLoadEnd);
            }

            //チェンバー変更の中断
            _isOpenCilinder = false;
            _isChangeCillinderPos = false;
            _countChangeChamberTime = 0;

            //リロードの中断
            _isReLoadNow = false;
            _isExcretedPods = false;
            _isSetBullet = false;
            _countSetBulletTime = 0;
            _countExcretedPodsTime = 0;
        }

        /// <summary> 空のチャンバーを見つける </summary>
        /// <returns> 空のチャンバーの位置。無い場合 -1を返す。 </returns>
        private int FindEmptyChamber()
        {
            int result = _playerController.Revolver.CurrentChamber;

            for (int i = 0; i < 6; i++)
            {
                if (_playerController.Revolver.
                    Cylinder[(result + i) % _playerController.Revolver.Cylinder.Length] == null)
                {
                    return (result + i) % _playerController.Revolver.Cylinder.Length;
                }
            }

            return -1;
        }
    }
}