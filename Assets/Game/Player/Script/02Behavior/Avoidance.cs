// 日本語対応
using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

namespace Player
{
    /// <summary>
    /// 回避クラス
    /// </summary>
    [System.Serializable]
    public class Avoidance
    {
        [Header("Test用、回避のText")]
        [SerializeField]
        private GameObject _testAvoidText;

        [Tooltip("回避中の時間の速度"), SerializeField]
        private float _timeScale = 0.7f;

        [Header("回避を適用するボタンの押し込み時間")]
        [Tooltip("回避を適用するボタンの押し込み時間"), SerializeField]
        private float _doAvoidanceTime = 0.69f;

        [Header("回避を行う時間")]
        [Tooltip("回避を行う時間"), SerializeField]
        private float _avoidanceDoTime = 1f;

        [Header("回避のクールタイム")]
        [Tooltip("回避のクールタイム"), SerializeField]
        private float _avoidanceCoolTime = 4f;

        [Header("減速値")]
        [Tooltip("減速度（地上）"), SerializeField]
        private float _landDeceleration = 20f;

        [Header("回避した時のレイヤー")]
        [SerializeField] private string _avoidLayerName;
        [Header("最初のレイヤー")]
        [SerializeField] private string _defultLayerName;

        [Tooltip("現在の速度 : インスペクタで値を追跡する用"), SerializeField]
        private float _currentHorizontalSpeed = 0f;
        /// <summary>回避、の実行時間の計測用</summary>
        private float _avoidanceDoTimeCount = 0f;

        /// <summary>回避、のクールタイムの計測用</summary>
        private float _avoidanceCoolTimeCount = 0f;
        /// <summary>時遅、のクールタイムの計測用</summary>
        /// 
        /// <summary>現在、回避をしているかどうかを示す</summary>
        private bool _isAvoidacneNow = false;

        /// <summary>回避実行可能かどうか</summary>
        private bool _isCanAvoidance = true;

        private PlayerController _playerController = null;

        public bool IsAvoidanceNow => _isAvoidacneNow;

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
        }

        public void Init(PlayerController playerController)
        {
            _playerController = playerController;
            // 自分と無視するレイヤーのインデックスを取得
        }


        public void Update()
        {
            //Pause中は実行しない
            if (IsPause) return;

            //近接攻撃中は出来ない
            if (_playerController.Proximity.IsProximityNow) return;

            //入力を受けた
            if (_playerController.InputManager.IsPressed[InputType.Avoidance])
            {
                //( 回避が不可能　||　回避実行中 )には実行しない
                if (!_isCanAvoidance || _isAvoidacneNow) return;
                //ジャンプ入力と同フレームで入力した際に、上昇しながら回避に入る問題を防ぐための処理
                if (_playerController.Rigidbody2D.velocity.y > 0f) return;
                //空中では実行できない
                if (!_playerController.GroungChecker.IsHit(_playerController.DirectionControler.MovementDirectionX)) return;


                //リロードを中断する
                _playerController.RevolverOperator.StopRevolverReLoad();

                //回避実行
                StartThereAvoidance();
            }

            //クールタイムを数える
            CountCoolTime();

            //回避、時遅、の実行時間を計測
            CountDoTime();

            //速度の減速(急停止)
            VelocityDeceleration();
        }

        /// <summary>回避、時遅の実行時間を計測する</summary>
        private void CountDoTime()
        {
            //回避実行中のタイムを計算
            if (_isAvoidacneNow)
            {
                _avoidanceDoTimeCount += Time.deltaTime;

                if (_avoidanceDoTimeCount >= _avoidanceDoTime)
                {
                    _isAvoidacneNow = false;

                    _avoidanceDoTimeCount = 0;
                    EndThereAvoidance();
                }
            }
        }

        /// <summary>回避、時遅のクールタイムを数える</summary>
        private void CountCoolTime()
        {
            if (!_isCanAvoidance && !_isAvoidacneNow)
            {
                _avoidanceCoolTimeCount += Time.deltaTime;
                if (_avoidanceCoolTimeCount > _avoidanceCoolTime)
                {
                    _isCanAvoidance = true;
                    _avoidanceCoolTimeCount = 0;
                }
            }
        }

        /// <summary>プレイヤーが急に回避した際の減速処理</summary>
        private void VelocityDeceleration()
        {
            if (_isAvoidacneNow)
            {
                _playerController.Move.VelocityDeceleration();
            }
        }

        /// <summary>
        /// その場回避開始処理
        /// </summary>
        private void StartThereAvoidance()
        {
            //TestTExT???????????????????///////////////////////////////////////
            _testAvoidText.SetActive(true);

            _isCanAvoidance = false;

            _isAvoidacneNow = true;

            //時遅を強制解除
            _playerController.GunSetUp.EmergencyStopSlowTime();

            //構えはじめている最中は、構えはじめを解除
            _playerController.GunSetUp.CanselSetUpping();

            //回避の音
            GameManager.Instance.AudioManager.PlaySE("CueSheet_Gun", "SE_Player_Evade");

            _playerController.Player.layer = LayerMask.NameToLayer(_avoidLayerName);
            _playerController.LifeController.IsAvoid = true;
        }
        

        /// <summary>
        /// その場回避終了処理
        /// </summary>
        private void EndThereAvoidance()
        {
            //TestTExT???????????????????///////////////////////////////////////
            _testAvoidText.SetActive(false);

            //特定行動中に構えを解除していないかどうかを確認する
            _playerController.GunSetUp.CheckRelesedSetUp();

            _isAvoidacneNow = false;

            _playerController.LifeController.IsAvoid = false;

            _playerController.Player.layer = LayerMask.NameToLayer(_defultLayerName);
            ////回避が終了したことをMoveクラスに伝える
            _playerController.Move.EndOtherAction();

            _isAvoidacneNow = false;
        }
    }
}