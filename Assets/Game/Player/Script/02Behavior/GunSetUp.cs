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

        [Header("Test用、時遅いのText")]
        [SerializeField]
        private GameObject _testSlowTimeText;

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

        private bool _isReserveSetUpAvoid = false;

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


            // 近接攻撃中、
            if (_playerController.Proximity.IsProximityNow) return;

            if (_playerController.Avoidance.IsAvoidanceNow && !_playerController.GunSetUp.IsGunSetUp && _playerController.InputManager.IsPressed[InputType.GunSetUp] && !_isReserveSetUpAvoid)
            {
                _isReserveSetUpAvoid = true;
            }

            if (_isReserveSetUpAvoid)
            {
                if (!_playerController.PlayerAnimatorControl.IsAnimationNow)
                {
                    _isReserveSetUpAvoid = false;

                    //重力を戻す
                    _playerController.Rigidbody2D.gravityScale = 1f;

                    _isGunSetUp = true;
                    _isGunSetUping = false;

                    if (!_isSlowTimeNow)
                    {
                        DoSlow();
                    }

                    _playerController.PlayerAnimatorControl.GunSet(true);
                }
            }



            //構えの入力を離した場合
            if (_playerController.InputManager.IsReleased[InputType.GunSetUp])
            {
                EndSlowTime();

                //アニメーションを設定
                _playerController.PlayerAnimatorControl.GunSetEnd();

                //構えにかかる時間の計測をリセット
                _setUpTimeCount = 0;

                //構え、状態を解除
                _isGunSetUp = false;

                //構え中、の状態を解除
                _isGunSetUping = false;

                //緊急で解除する、をFalseに
                _isEmergencyStopSlowTime = false;

                //構え中、のboolをfalseに
                _isCanselSutUping = false;


                _playerController.Avoidance.EndThereAvoidance();
            }

            //構えのボタンを押したら
            if (_playerController.InputManager.IsPressed[InputType.GunSetUp] )
            {
                //構えてる最中
                _isGunSetUping = true;
            }


            //構えてはじめている間の処理。一定時間構えボタンを押していたら構える
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

                    DoSlow();
                    _playerController.PlayerAnimatorControl.GunSet(false);

                    _isNoGage = false;
                }

                //速度の減速処理
                _playerController.Move.VelocityDeceleration();

            }   //構えボタンを押したら構える


            //構えている最中の処理 
            //構えてる間、ゲージを減らす
            if (_isGunSetUp)
            {
                //速度の減速処理
                _playerController.Move.VelocityDeceleration();

            }       //構えてないときの処理。ゲージを回復する
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

                    EndSlowTime();

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
