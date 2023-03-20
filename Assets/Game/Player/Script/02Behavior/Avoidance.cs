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
        [Tooltip("回避中の時間の速度"), SerializeField]
        private float _timeScale = 0.7f;

        [Header("回避を適用するボタンの押し込み時間")]
        [Tooltip("回避を適用するボタンの押し込み時間"), SerializeField]
        private float _doAvoidanceTime = 0.69f;

        [Header("回避のクールタイム")]
        [Tooltip("回避のクールタイム"), SerializeField]
        private float _avoidanceCoolTime = 4f;

        [Header("時遅のクールタイム")]
        [Tooltip("時遅のクールタイム"), SerializeField]
        private float _slowTimeCoolTime = 10f;

        [Header("減速値")]
        [Tooltip("減速度（地上）"), SerializeField]
        private float _landDeceleration = 20f;

        [Tooltip("現在の速度 : インスペクタで値を追跡する用"), SerializeField]
        private float _currentHorizontalSpeed = 0f;

        [Header("Test用。後で消す")]
        [SerializeField] private SpriteRenderer _spriteRenderer;

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
        private bool _isCanAvoidance = false;

        /// <summary>時遅が実行可能かどうか</summary>
        private bool _isCanSlowTime = false;

        private bool _isExecutionNow = false;

        private PlayerController _playerController = null;

        public bool IsAvoiddanceNow => _isAvoidacneNow;

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

        public async void Update()
        {
            //Pause中は実行しない
            if (IsPause) return;

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
                await UniTask.Delay(8000); // とりあえず一秒待つ
                EndTimeSlow();
            }

            // 回避入力が発生したときに処理を実行する
            if (!_isExecutionNow &&
                _isDoAvoidance
                && _playerController.GroungChecker.IsHit(_playerController.DirectionControler.MovementDirectionX))
            {
                _isExecutionNow = true;

                _isDoAvoidance = false;

                _isCanAvoidance = false;

                StartThereAvoidance();
                // 回避が完了するまで待機する
                // await UniTask.WaitUntil(() => true);
                await UniTask.Delay(1000); // とりあえず一秒待つ
                EndThereAvoidance();

                // 入力を開放するまで待機
                await UniTask.WaitUntil(() => _playerController.InputManager.GetValue<float>(InputType.Avoidance) < 0.01f);
                _isExecutionNow = false;
            }
            //速度の減速
            VelocityDeceleration();
        }

        /// <summary>ボタンの押し込み時間を計算する</summary>
        private void CheckInputButtun()
        {
            if (_playerController.InputManager.IsExist[InputType.Avoidance])
            {
                _countL2Time += Time.deltaTime;
            }
            else if (_playerController.InputManager.IsReleased[InputType.Avoidance])
            {
                //押した時間が、回避適用時間より下だったら回避
                //そうじゃなかったら時を遅くする
                if (_countL2Time > _doAvoidanceTime)
                {
                    if (_isCanSlowTime) _isDoSlowTime = true;
                }
                else
                {
                    if (_isCanAvoidance) _isDoAvoidance = true;
                }
                _countL2Time = 0;
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
                _currentHorizontalSpeed = _playerController.Rigidbody2D.velocity.x;

                if (_playerController.GroungChecker.IsHit(_playerController.DirectionControler.MovementDirectionX))
                {
                    _currentHorizontalSpeed -= Time.deltaTime * _landDeceleration * _playerController.Player.transform.localScale.x;
                } // 地上移動中の減速処理

                if (_playerController.Player.transform.localScale.x > 0f && _currentHorizontalSpeed < 0f ||
                    _playerController.Player.transform.localScale.x < 0f && _currentHorizontalSpeed > 0f)
                {
                    _currentHorizontalSpeed = 0f;
                } // 「右」に向いている状態で減速するときは0より小さくならない、
                  // 「左」に向いている状態で減速するときは0より大きくならない。


                // 速度を割り当てる。
                _playerController.Rigidbody2D.velocity =
                        new Vector2(_currentHorizontalSpeed,
                        _playerController.Rigidbody2D.velocity.y);
            }
        }

        /// <summary>
        /// その場回避開始処理
        /// </summary>
        private void StartThereAvoidance()
        {
            //テスト用で、回避を分かりやすくするために使用
            _spriteRenderer.color = Color.red;

            Debug.Log("その場回避始め！");
            _playerController.LifeController.IsGodMode = true;
            Physics2D.IgnoreLayerCollision(_ignoreLayerIndex, _myLayerIndex, true);

            _isAvoidacneNow = true;
        }
        /// <summary>
        /// その場回避終了処理
        /// </summary>
        private void EndThereAvoidance()
        {
            //テスト用で、回避を分かりやすくするために使用
            _spriteRenderer.color = Color.black;

            Debug.Log("その場回避終了！");
            _playerController.LifeController.IsGodMode = false;
            Physics2D.IgnoreLayerCollision(_ignoreLayerIndex, _myLayerIndex, false);

            //回避が終了したことをMoveクラスに伝える
            _playerController.Move.EndAvoidance();

            _isAvoidacneNow = false;
        }

        private void StartTimeSlow()
        {
            Debug.Log("時を遅くする");
            // 時間の速度をゆっくりにする。
            GameManager.Instance.TimeController.ChangeTimeSpeed(_timeScale);
            _isSlowTimeNow = true;
        }

        private void EndTimeSlow()
        {
            Debug.Log("時を戻す");
            // 時間の速度をもとの状態に戻す。
            GameManager.Instance.TimeController.ChangeTimeSpeed(1f);
            _isSlowTimeNow = false;
        }
    }
}