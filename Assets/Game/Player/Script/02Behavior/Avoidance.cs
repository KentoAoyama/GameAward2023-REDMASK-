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

        [Header("Test用、時遅いのText")]
        [SerializeField]
        private GameObject _testSlowTimeText;


        [Tooltip("回避中の時間の速度"), SerializeField]
        private float _timeScale = 0.7f;

        [Header("回避を適用するボタンの押し込み時間")]
        [Tooltip("回避を適用するボタンの押し込み時間"), SerializeField]
        private float _doAvoidanceTime = 0.69f;

        [Header("回避を行う時間")]
        [Tooltip("回避を行う時間"), SerializeField]
        private float _avoidanceDoTime = 1f;

        [Header("時遅を行う時間")]
        [Tooltip("時遅を行う時間"), SerializeField]
        private float _slowTimeDoTime = 5f;

        [Header("回避のクールタイム")]
        [Tooltip("回避のクールタイム"), SerializeField]
        private float _avoidanceCoolTime = 4f;

        [Header("時遅のクールタイム")]
        [Tooltip("時遅のクールタイム"), SerializeField]
        private float _slowTimeCoolTime = 10f;

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
        /// <summary>時遅、の実行時間の計測用</summary>
        private float _slowTimeDoTimeCount = 0f;

        /// <summary>回避、のクールタイムの計測用</summary>
        private float _avoidanceCoolTimeCount = 0f;
        /// <summary>時遅、のクールタイムの計測用</summary>
        private float _slowTimeCoolTimeCount = 0f;
        /// <summary>L2ボタンを押している時間の計測用</summary>
        private float _countL2Time = 0;

        private int _myLayerIndex = default;

        private int _ignoreLayerIndex = default;

        /// <summary>現在、回避をしているかどうかを示す</summary>
        private bool _isAvoidacneNow = false;

        /// <summary>現在、時遅をしているかどうかを示す</summary>
        private bool _isSlowTimeNow = false;

        /// <summary>回避実行可能かどうか</summary>
        private bool _isDoAvoidance = false;

        /// <summary>時遅が実行可能かどうか</summary>
        private bool _isDoSlowTime = false;

        /// <summary>回避実行可能かどうか</summary>
        private bool _isCanAvoidance = true;

        /// <summary>時遅が実行可能かどうか</summary>
        private bool _isCanSlowTime = true;

        private PlayerController _playerController = null;

        public bool IsAvoidanceNow => _isAvoidacneNow;

        public bool IsDoAvoidance => _isDoAvoidance;

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
            _ignoreLayerIndex = LayerMask.NameToLayer("EnemyBullet"); // ここの文字列を消したい
            _myLayerIndex = LayerMask.NameToLayer("Player"); // ここの文字列を消したい
        }


        public void Update()
        {
            //Pause中は実行しない
            if (IsPause) return;

            //近接攻撃中は出来ない
            if (_playerController.Proximity.IsProximityNow) return;


            //ボタンの押し込み時間を計測
            CheckInputButtun();
            //クールタイムを数える
            CountCoolTime();

            //時間を遅くする処理を実行
            if (_isDoSlowTime)
            {
                _isDoSlowTime = false;
                _isCanSlowTime = false;
                StartTimeSlow();
            }

            // 回避入力が発生したときに処理を実行する
            if (!_isAvoidacneNow &&
                _isDoAvoidance
                && _playerController.GroungChecker.IsHit(_playerController.DirectionControler.MovementDirectionX))
            {
                //ジャンプ入力と同フレームで入力した際に、上昇しながら回避に入る問題を防ぐための処理
                if (_playerController.Rigidbody2D.velocity.y > 0f)
                {
                    return;
                }

                _isDoAvoidance = false;

                _isAvoidacneNow = true;

                _isCanAvoidance = false;

                StartThereAvoidance();
            }
            else if (_isDoAvoidance && !_playerController.GroungChecker.IsHit(_playerController.DirectionControler.MovementDirectionX))
            {
                _isDoAvoidance = false;
            }

            //回避、時遅、の実行時間を計測
            CountDoTime();

            //速度の減速(急停止)
            VelocityDeceleration();
        }

        /// <summary>ボタンの押し込み時間を計算する</summary>
        private void CheckInputButtun()
        {
            if (_playerController.InputManager.IsExist[InputType.Avoidance])
            {
                _countL2Time += Time.deltaTime;

                if (_countL2Time > _doAvoidanceTime)
                {
                    if (_isCanSlowTime)
                    {
                        _isDoSlowTime = true;

                        _countL2Time = 0;

                        //リロードを中断する
                        _playerController.RevolverOperator.StopRevolverReLoad();
                    }
                }
            }
            else if (_playerController.InputManager.IsReleased[InputType.Avoidance])
            {
                //押した時間が、回避適用時間より下だったら回避
                //そうじゃなかったら時を遅くする
                if (_countL2Time <= _doAvoidanceTime)
                {
                    if (_isCanAvoidance)
                    {
                        _isDoAvoidance = true;
                        _countL2Time = 0;

                        //リロードを中断する
                        _playerController.RevolverOperator.StopRevolverReLoad();
                    }
                }
            }
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
                    _avoidanceDoTimeCount = 0;
                    EndThereAvoidance();
                }
            }

            if (_isSlowTimeNow)
            {
                _slowTimeDoTimeCount += Time.deltaTime;

                if (_slowTimeDoTimeCount > _slowTimeDoTime)
                {
                    _slowTimeDoTimeCount = 0;
                    EndTimeSlow();
                }
            }

        }

        /// <summary>回避、時遅のクールタイムを数える</summary>
        private void CountCoolTime()
        {
            if (!_isCanSlowTime && !_isSlowTimeNow)
            {
                _slowTimeCoolTimeCount += Time.deltaTime;
                if (_slowTimeCoolTimeCount > _slowTimeCoolTime)
                {
                    _isCanSlowTime = true;
                    _slowTimeCoolTimeCount = 0;
                }
            }

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

            Debug.Log("その場回避始め！");


            _playerController.Player.layer = LayerMask.NameToLayer(_avoidLayerName);
            _playerController.LifeController.IsGodMode = true;
        }
        /// <summary>
        /// その場回避終了処理
        /// </summary>
        private void EndThereAvoidance()
        {
            //TestTExT???????????????????///////////////////////////////////////
            _testAvoidText.SetActive(false);

            Debug.Log("その場回避終了！");
            _playerController.LifeController.IsGodMode = false;

            _playerController.Player.layer = LayerMask.NameToLayer(_defultLayerName);
            ////回避が終了したことをMoveクラスに伝える
            _playerController.Move.EndOtherAction();

            _isAvoidacneNow = false;
        }

        private void StartTimeSlow()
        {
            /////TEST用............
            _testSlowTimeText.SetActive(true);

            GameManager.Instance.ShaderPropertyController.MonochromeController.SetMonoBlend(1, 0.2f);

            Debug.Log("時を遅くする");
            // 時間の速度をゆっくりにする。
            GameManager.Instance.TimeController.ChangeTimeSpeed(true);
            _isSlowTimeNow = true;
        }

        private void EndTimeSlow()
        {
            /////TEST用............
            _testSlowTimeText.SetActive(false);

            GameManager.Instance.ShaderPropertyController.MonochromeController.SetMonoBlend(0, 0.2f);

            Debug.Log("時を戻す");
            // 時間の速度をもとの状態に戻す。
            GameManager.Instance.TimeController.ChangeTimeSpeed(false);
            _isSlowTimeNow = false;
        }
    }
}