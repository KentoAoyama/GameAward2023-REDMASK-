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

        private bool _isSlowTimeNow = false;

        private int _beatSoundIndex = -1;

        public bool IsSlowTimeNow => _isSlowTimeNow;

        private PlayerController _playerController = null;

        private bool _isEmergencyStopSlowTime = false;

        /// <summary>構えているかどうか</summary>
        private bool _isGunSetUp = false;

        public bool IsGunSetUp => _isGunSetUp;

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

            AnimEndSetUpCheck();
        }

        public void Init(PlayerController playerController)
        {
            _playerController = playerController;
        }


        /// <summary>
        /// 各行動終了時に、構えを話しているかどうかを確認する
        /// </summary>
        public void AnimEndSetUpCheck()
        {
            //死んだら何もしない
            if (_playerController.IsDead) return;

            _isEmergencyStopSlowTime = false;

            if (_playerController.InputManager.IsExist[InputType.GunSetUp])
            {
                _isGunSetUp = true;

                if (!_isSlowTimeNow)
                {
                    DoSlow();
                }

                //腕を出す
                _playerController.PlayerAnimatorControl.GunSet(true);

                //腕の角度を設定
                _playerController.BodyAnglSetteing.AimingSet();

                //照準を描写
                _playerController.Revolver.OnDrawAimingLine();

            }
            else
            {
                EndSlowTime();

                //重力を戻す
                _playerController.Rigidbody2D.gravityScale = 1f;

                _isGunSetUp = false;

                _playerController.PlayerAnimatorControl.GunSetEnd();
            }

        }

        public void UpData()
        {
            // ポーズ中は何もできない
            if (GameManager.Instance.PauseManager.PauseCounter > 0) return;

            //死んだら何もしない
            if (_playerController.IsDead) return;

            //構えている最中の処理 
            //構えてる間、ゲージを減らす
            if (_isGunSetUp)
            {
                //速度の減速処理
                _playerController.Move.VelocityDeceleration();

            }       //構えてないときの処理。ゲージを回復する



            //空中では出来ない
            if (!_playerController.GroungChecker.IsHit(_playerController.Move.MoveHorizontalDir))
            {
                if (_isGunSetUp)
                {
                    if (_playerController.Avoidance.IsAvoidanceNow)
                    {
                        _playerController.Avoidance.EndThereAvoidance();
                    }

                    //重力を戻す
                    _playerController.Rigidbody2D.gravityScale = 1f;

                    //時遅を解
                    EndSlowTime();

                    //アニメーションを設定
                    _playerController.PlayerAnimatorControl.GunSetEnd();

                    //構え、状態を解除
                    _isGunSetUp = false;

                    //緊急で解除する、をFalseに
                    _isEmergencyStopSlowTime = false;

                    //回避を終了
                    _playerController.Avoidance.EndThereAvoidance();
                }
                return;
            }

            // 近接攻撃中、は出来ない
            if (_playerController.Proximity.IsProximityNow) return;

            if (_playerController.PlayerAnimatorControl.IsAnimationNow) return;

            //回避中の構えの入力受付
            //if (_playerController.Avoidance.IsAvoidanceNow && )
            //{
            //    return;
            //}

            //構えの入力を離した場合
            if (_playerController.InputManager.IsReleased[InputType.GunSetUp] && _isGunSetUp)
            {
                if (_playerController.Avoidance.IsAvoidanceNow)
                {
                    _playerController.Avoidance.EndThereAvoidance();
                }

                //重力を戻す
                _playerController.Rigidbody2D.gravityScale = 1f;

                //時遅を解
                EndSlowTime();

                //アニメーションを設定
                _playerController.PlayerAnimatorControl.GunSetEnd();

                //構え、状態を解除
                _isGunSetUp = false;

                //緊急で解除する、をFalseに
                _isEmergencyStopSlowTime = false;

                //回避を終了
                _playerController.Avoidance.EndThereAvoidance();
            }

            //構えのボタンを押したら
            if (_playerController.InputManager.IsPressed[InputType.GunSetUp] && !_isGunSetUp)
            {
                _isGunSetUp = true;

                //リロードを中断する
                _playerController.RevolverOperator.StopRevolverReLoad(false);

                //構え
                _playerController.PlayerAnimatorControl.GunSet(false);

                //腕の角度を設定
                _playerController.BodyAnglSetteing.AimingSet();

                //照準を描写
                _playerController.Revolver.OnDrawAimingLine();

                DoSlow();
            }
        }


        ///// <summary>各行動終了後</summary>
        ///// 構えがボタンを話したかどうかを確認する
        //public void CheckRelesedSetUp()
        //{
        //    //死んだら何もしない
        //    if (_playerController.IsDead) return;

        //    if (!_playerController.InputManager.IsExist[InputType.GunSetUp])
        //    {
        //        if (_isGunSetUp)
        //        {
        //            _isGunSetUp = false;

        //            _playerController.PlayerAnimatorControl.GunSetEnd();

        //            EndSlowTime();

        //            _isEmergencyStopSlowTime = false;
        //        }

        //    }     //構えボタンを離したら構えを解除
        //}

        public void DoSlow()
        {
            _playerController.Rigidbody2D.gravityScale = 0;

            _isSlowTimeNow = true;

            GameManager.Instance.AudioManager.PlaySE("CueSheet_Gun", "SE_Player_Slow");
            _beatSoundIndex = GameManager.Instance.AudioManager.PlaySE("CueSheet_Gun", "SE_HeartBeat");

            GameManager.Instance.ShaderPropertyController.MonochromeController.SetMonoBlend(1, 0.2f);

            Debug.Log("時を遅くする");
            // 時間の速度をゆっくりにする。
            GameManager.Instance.TimeController.ChangeTimeSpeed(true);
        }


        public void EndSlowTime()
        {
            if (_isEmergencyStopSlowTime || !_isGunSetUp) return;

            _playerController.Rigidbody2D.gravityScale = 1;

            _isSlowTimeNow = false;

            //遅くする時の音
            GameManager.Instance.AudioManager.PlaySE("CueSheet_Gun", "SE_Player_SlowFinish");
            GameManager.Instance.AudioManager.StopSE(_beatSoundIndex);
            _beatSoundIndex = -1;

            GameManager.Instance.ShaderPropertyController.MonochromeController.SetMonoBlend(0, 0.2f);

            Debug.Log("時を戻す");
            // 時間の速度をもとの状態に戻す。
            GameManager.Instance.TimeController.ChangeTimeSpeed(false);
        }

        public void StopSlowSE()
        {
            GameManager.Instance.AudioManager.StopSE(_beatSoundIndex);
            _beatSoundIndex = -1;
        }

        public void CanselSetUpping()
        {
            _playerController.PlayerAnimatorControl.GunSetEnd();

            _isEmergencyStopSlowTime = false;

            EndSlowTime();

            _isGunSetUp = false;
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
