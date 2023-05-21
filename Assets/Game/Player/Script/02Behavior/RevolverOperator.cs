
using Bullet;
using UI;
using UnityEngine;


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

        [Header("排莢にかかる時間")]
        [SerializeField] private float _setBulletTime = 1;

        [Header("マズルフラッシュ")]
        [SerializeField] private MuzzleFlashController _muzzleFlash;


        private float _countExcretedPodsTime = 0;

        private float _countSetBulletTime = 0;

        private bool _isExcretedPods = false;
        private bool _isSetBullet = false;


        private bool _isReLoadNow = false;

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

            if (_playerController.Proximity.IsProximityNow)
            {
                return;
            }//近接攻撃中はできない


            if (_playerController.GunSetUp.IsGunSetUp)
            {
                // 撃てる状態かつ、撃つ入力が発生したとき 銃を撃つ
                if (_playerController.InputManager.GetValue<float>(InputType.Fire1) > 0.49f &&
                    _playerController.Revolver.CanFire)
                {
                    //アニメーションの再生
                    if (_playerController.Avoidance.IsAvoidanceNow)
                    {
                        _playerController.PlayerAnimatorControl.PlayAnimation(PlayerAnimationControl.AnimaKind.AvoidFire);
                    }
                    else
                    {
                        _playerController.PlayerAnimatorControl.PlayAnimation(PlayerAnimationControl.AnimaKind.Fire);
                    }


                    ReactionMessageSender.SendMessage(_playerController.Player.transform);

                    //マズルフラッシュを再生
                    _muzzleFlash.PlayMuzzleFlash();

                    //発砲処理
                    _playerController.Revolver.Fire();

                    //リロードの中断処理
                    StopRevolverReLoad();

                    //特定行動中に構えを解除していないかどうかを確認する
                    _playerController.GunSetUp.CheckRelesedSetUp();

                    return;
                }
            }

            if (_playerController.Avoidance.IsAvoidanceNow)
            {
                return;
            } //回避中はできない

            // リロード処理
            if (_playerController.InputManager.IsPressed[InputType.LoadBullet])
            {
                //排出、弾を籠めている最中に押して何度も呼ばれないようにする
                if (_isExcretedPods || _isSetBullet) return;

                _playerController.Revolver.OffDrawAimingLine(false);

                _isReLoadNow = true;

                //時遅を強制解除
                _playerController.GunSetUp.EmergencyStopSlowTime();

                //構えはじめている最中は、構えはじめを解除
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

                    // 排莢する
                    var cylinder = _playerController.Revolver.EjectShellsAll();
                    _countExcretedPodsTime = 0;

                    _isExcretedPods = false;

                    // 空いているチャンバーを検索する。
                    var index = FindEmptyChamber();
                    if (index != -1) // 空いているチャンバーが見つかった場合の処理
                    {
                        _isSetBullet = true;
                    }
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
                    _playerController.GunSetUp.CheckRelesedSetUp();

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




        /// <summary>他の行動をしたことによるリロードの中断</summary>
        public void StopRevolverReLoad()
        {
            /////////////////////////////////TEST用!!!!!!!!!!!!!!!!//////////////////////////
            _excretedText.SetActive(false);
            _setBulletText.SetActive(false);

            if (_isReLoadNow)
            {
                //弾を籠めるアニメーション
                _playerController.PlayerAnimatorControl.PlayAnimation(PlayerAnimationControl.AnimaKind.ReLoadEnd);
            }
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