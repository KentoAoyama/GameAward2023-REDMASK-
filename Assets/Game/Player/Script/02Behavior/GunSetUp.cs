using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

namespace Player
{
    [System.Serializable]
    public class GunSetUp
    {
        [Header("構えるまでの時間(秒)")]
        [SerializeField] private float _setUpTime = 0.2f;

        [Header("ゲージの最大値(秒)")]
        [SerializeField] private float _gageMax = 5f;

        [Header("ゲージ回復の速さ。1秒*この値だよ")]
        [SerializeField] private float _gageHealPercent = 1f;

        [Header("ゲージを回復するまでの空き時間")]
        [SerializeField] private float _gageHealWaitTime = 1f;

        [Header("ゲージ用のスライダー")]
        [SerializeField] private Slider _gageSlider;

        [Header("Test用、時遅いのText")]
        [SerializeField]
        private GameObject _testSlowTimeText;

        /// <summary>現在のゲージ量</summary>
        private float _nowGage = 0;

        private float _gageHealWaitTimeCount = 0;

        private float _setUpTimeCount = 0;

        private bool _isSlowTimeNow = false;

        public bool IsSlowTimeNow => _isSlowTimeNow;

        private PlayerController _playerController = null;

        private bool _isEmergencyStopSlowTime = false;

        private bool _isCanselSutUping = false;

        /// <summary>構えているかどうか</summary>
        private bool _isGunSetUp = false;

        private bool _isGunSetUping = false;

        private bool _isNoGage = false;
        public bool IsGunSetUp => _isGunSetUp;

        public bool IsGunSetUpping => _isGunSetUping;


        public bool IsPause { get; private set; } = false;

        public async void Pause()
        {
            await UniTask.WaitUntil(() => _playerController != null);
            // Updateを停止する
            IsPause = true;
        }
        public void Resume()
        {
            // Updateを再開する
            IsPause = false;

            CheckRelesedSetUp();


        }

        public void Init(PlayerController playerController)
        {
            _playerController = playerController;

            _nowGage = _gageMax;

            _gageSlider.maxValue = _gageMax;
        }


        public void UpData()
        {
            if (GameManager.Instance.PauseManager.PauseCounter > 0)
            {
                return;
            } // ポーズ中は何もできない

            if (!_playerController.GroungChecker.IsHit(_playerController.Move.MoveHorizontalDir))
            {
                return;
            }   //空中では出来ない


            //
            if (_playerController.Proximity.IsProximityNow || _playerController.Avoidance.IsAvoidanceNow) return;


            //構えの入力を離した場合
            if (_playerController.InputManager.IsReleased[InputType.GunSetUp])
            {
                //アニメーションを設定
                _playerController.PlayerAnimatorControl.GunSetEnd();

                //ゲージが余っていた時に時遅を解除
                if (_nowGage > 0)
                {
                    EndSlowTime();
                }

                //構えにかかる時間の計測をリセット
                _setUpTimeCount = 0;

                //ゲージを回復するまでの計測をリセット
                _gageHealWaitTimeCount = 0;

                //構え、状態を解除
                _isGunSetUp = false;

                //構え中、の状態を解除
                _isGunSetUping = false;

                //緊急で解除する、をFalseに
                _isEmergencyStopSlowTime = false;

                //構え中、のboolをfalseに
                _isCanselSutUping = false;

            }

            //構えのボタンを押したら
            if (_playerController.InputManager.IsPressed[InputType.GunSetUp])
            {
                //構えてる最中
                _isGunSetUping = true;
            }


            if (_isGunSetUping)
            {
                if (_isGunSetUp || _isCanselSutUping)
                {
                    return;
                }

                _setUpTimeCount += Time.deltaTime;

                if (_setUpTimeCount >= _setUpTime)
                {
                    //重力を戻す
                    _playerController.Rigidbody2D.gravityScale = 1f;

                    _isGunSetUp = true;
                    _isGunSetUping = false;

                    if (_nowGage > 0)
                    {
                        DoSlow();
                        _playerController.PlayerAnimatorControl.GunSet();

                        _isNoGage = false;
                    }
                }

            }   //構えボタンを押したら構える



            if (_isGunSetUp)
            {
                if (!_isNoGage)
                {
                    _nowGage -= Time.deltaTime;

                    _gageSlider.value = _nowGage;

                    if (_nowGage <= 0)
                    {
                        _nowGage = 0;

                        EndSlowTime();
                    }
                }

            }   //構えてる間、ゲージを減らす
            else
            {
                if (_gageHealWaitTimeCount < _gageHealWaitTime)
                {
                    _gageHealWaitTimeCount += Time.deltaTime;

                }   //ゲージを回復するまでの時間を計測
                else
                {
                    if (_nowGage < _gageMax)
                    {
                        //ゲージを回復
                        _nowGage += Time.deltaTime * _gageHealPercent;

                        _gageSlider.value = _nowGage;


                        if (Mathf.Abs(_gageMax - _nowGage) < 0.3f)
                        {
                            _nowGage = _gageMax;
                        }
                    }
                }
            }
        }


        public void CheckRelesedSetUp()
        {
            if (!_playerController.InputManager.IsExist[InputType.GunSetUp])
            {
                if (_isGunSetUp)
                {
                    _playerController.PlayerAnimatorControl.GunSetEnd();

                    _isGunSetUp = false;

                    _isGunSetUping = false;

                    _setUpTimeCount = 0;

                    _gageHealWaitTimeCount = 0;

                    if (_nowGage > 0)
                    {
                        EndSlowTime();
                    }

                    _isEmergencyStopSlowTime = false;
                    _isCanselSutUping = false;
                }

            }     //構えボタンを離したら構えを解除
        }

        public void DoSlow()
        {
            /////TEST用............
            _testSlowTimeText.SetActive(true);

            _isSlowTimeNow = true;

            GameManager.Instance.AudioManager.PlaySE("CueSheet_Gun", "SE_Player_Slow");

            GameManager.Instance.ShaderPropertyController.MonochromeController.SetMonoBlend(1, 0.2f);

            Debug.Log("時を遅くする");
            // 時間の速度をゆっくりにする。
            GameManager.Instance.TimeController.ChangeTimeSpeed(true);
        }


        public void EndSlowTime()
        {
            if (_isEmergencyStopSlowTime || !_isGunSetUp) return;

            /////TEST用............
            _testSlowTimeText.SetActive(false);

            _isSlowTimeNow = false;

            _isNoGage = true;

            //遅くする時の音
            GameManager.Instance.AudioManager.PlaySE("CueSheet_Gun", "SE_Player_SlowFinish");

            GameManager.Instance.ShaderPropertyController.MonochromeController.SetMonoBlend(0, 0.2f);

            Debug.Log("時を戻す");
            // 時間の速度をもとの状態に戻す。
            GameManager.Instance.TimeController.ChangeTimeSpeed(false);
        }

        public void CanselSetUpping()
        {
            if (_isGunSetUp || _isCanselSutUping) return;

            _isGunSetUping = false;
            _isCanselSutUping = true;
        }

        public void EmergencyStopSlowTime()
        {
            //構えている状態じゃなかったら呼ばない
            if (!_isGunSetUp) return;

            //時遅中でなかったら呼ばない
            if (!_isSlowTimeNow) return;

            //既にこの関数を読んでいたら呼ばない
            if (_isEmergencyStopSlowTime) return;



            EndSlowTime();

            _isEmergencyStopSlowTime = true;

        }

    }


}
