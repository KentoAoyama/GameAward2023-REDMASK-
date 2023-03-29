using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    [System.Serializable]
    public class Move
    {
        [Header("水平移動")]
        [Header("地上 : 移動開始")]
        [Tooltip("低速移動速度"), SerializeField]
        private float _startSpeed = 1f;
        [Tooltip("標準移動に切り替えるまでの時間"), SerializeField]
        private float _toMoveTime = 0.2f;
        [Header("地上 : 標準移動速度"), SerializeField]
        private float _moveSpeed = 4f;
        [Header("空中水平移動制御")]
        [Tooltip("空中 : 移動加速度"), SerializeField]
        private float _midairMoveAcceleration = 4f;
        [Tooltip("空中 : 移動最大速度"), SerializeField]
        private float _maxMidairMoveSpeed = 4f;
        [Header("減速値")]
        [Tooltip("減速度（地上）"), SerializeField]
        private float _landDeceleration = 20f;
        [Tooltip("減速度（空中）"), SerializeField]
        private float _midairDeceleration = 10f;
        [Header("現在の速度")]
        [Tooltip("現在の速度 : インスペクタで値を追跡する用"), SerializeField]
        private float _currentHorizontalSpeed = 0f;

        [Header("垂直移動")]
        [Tooltip("ジャンプ力"), SerializeField]
        private float _jumpPower = 4f;

        private float _toMoveTimer = 0f;
        private HorizontalMoveMode _currentHorizontalMoveMode = HorizontalMoveMode.Stop;
        private float _previousDir = 0f;
        private float _moveHorizontalDir = 1f;



        private PlayerController _playerController;

        /// <summary>
        /// 移動できるかどうかを表す値 <br/>
        /// この値が trueであれば、移動入力が発生したときに移動処理を実行する。
        /// </summary>
        public bool CanMove { get; set; } = true;
        /// <summary>
        /// ジャンプできるかどうかを表す値 <br/>
        /// この値が trueであれば、ジャンプ入力が発生したときにジャンプ処理を実行する。
        /// </summary>
        public bool CanJump { get; set; } = true;
        public bool IsPause { get; private set; } = false;

        public float MoveHorizontalDir => _moveHorizontalDir;


        public void Init(PlayerController playerController)
        {
            _playerController = playerController;
        }
        public void ClearHorizontalSpeed()
        {
            _currentHorizontalSpeed = 0f;
        }

        private Vector2 _velocity;
        private float _angularVelocity;
        public async void Pause()
        {
            await UniTask.WaitUntil(() => _playerController != null);
            // Updateを停止する
            IsPause = true;
            // Rigidbody2Dのポーズ処理
            _velocity = _playerController.Rigidbody2D.velocity;
            _angularVelocity = _playerController.Rigidbody2D.angularVelocity;
            _playerController.Rigidbody2D.Sleep();
            _playerController.Rigidbody2D.simulated = false;
        }
        public void Resume()
        {
            // Updateを再開する
            IsPause = false;
            // Rigidbody2Dのリジューム処理
            _playerController.Rigidbody2D.simulated = true;
            _playerController.Rigidbody2D.WakeUp();
            _playerController.Rigidbody2D.angularVelocity = _angularVelocity;
            _playerController.Rigidbody2D.velocity = _velocity;
        }
        public void Update()
        {
            //回避中は移動不可、の条件を追加した
            if (IsPause || _playerController.Avoidance.IsAvoidanceNow || _playerController.Proximity.IsProximityNow)
            {
                return;
            }

            var gameTime = GameManager.Instance.TimeController.PlayerTime;

            // 移動入力があるときの処理
            if (_playerController.InputManager.IsExist[InputType.MoveHorizontal] && CanMove)
            {
                // 1フレーム前の水平移動方向（入力方向）を保存しておく
                _previousDir = _moveHorizontalDir;
                // 方向を表す値を更新する。（右に相当する値なら1, 左に相当する値なら-1。）
                _moveHorizontalDir = _playerController.InputManager.GetValue<float>(InputType.MoveHorizontal) > 0f ? 1f : -1f;


                    _playerController.PlayerAnimatorControl.SetPlayerDir(_moveHorizontalDir);
                // 入力方向が切り替わった時の処理
                if (Mathf.Abs(_previousDir - _moveHorizontalDir) > 0.1f)
                {
                    //プレイヤーの向きを設定


                    _toMoveTimer = 0f;
                    _currentHorizontalMoveMode = HorizontalMoveMode.Start;
                }

                // 接地しているとき
                if (_playerController.GroungChecker.IsHit(_playerController.DirectionControler.MovementDirectionX))
                {
                    // プレイヤーの状態によって処理を変える。
                    switch (_currentHorizontalMoveMode)
                    {
                        case HorizontalMoveMode.Stop:
                            _toMoveTimer = 0f;
                            _currentHorizontalMoveMode = HorizontalMoveMode.Start;
                            break;
                        case HorizontalMoveMode.Start:
                            _currentHorizontalSpeed = _startSpeed * _moveHorizontalDir * gameTime;
                            _toMoveTimer += Time.deltaTime;
                            if (_toMoveTimer > _toMoveTime) _currentHorizontalMoveMode = HorizontalMoveMode.Move;
                            break;
                        case HorizontalMoveMode.Move:
                            _currentHorizontalSpeed = _moveSpeed * _moveHorizontalDir * gameTime;
                            break;
                        case HorizontalMoveMode.Deceleration:
                            _toMoveTimer = 0f;
                            _currentHorizontalMoveMode = HorizontalMoveMode.Start;
                            break;
                    }
                }
                // 接地していないとき
                else
                {
                    _currentHorizontalMoveMode = HorizontalMoveMode.Move;

                    _currentHorizontalSpeed += Time.deltaTime * _midairMoveAcceleration * _moveHorizontalDir * gameTime;
                    if (_currentHorizontalSpeed > _maxMidairMoveSpeed) _currentHorizontalSpeed = _maxMidairMoveSpeed; // 最大速度を超えない
                }
            }
            // 入力がないとき、現在速度を減算する。
            else
            {
                if (_playerController.GroungChecker.IsHit(_playerController.DirectionControler.MovementDirectionX))
                {
                    _currentHorizontalSpeed -= Time.deltaTime * _landDeceleration * _moveHorizontalDir * gameTime;
                } // 地上移動中の減速処理
                else
                {
                    _currentHorizontalSpeed -= Time.deltaTime * _midairDeceleration * _moveHorizontalDir * gameTime;
                } // 空中移動中の減速処理

                if (_moveHorizontalDir > 0f && _currentHorizontalSpeed < 0f ||
                    _moveHorizontalDir < 0f && _currentHorizontalSpeed > 0f)
                {
                    _currentHorizontalSpeed = 0f;
                } // 「右」に向いている状態で減速するときは0より小さくならない、
                  // 「左」に向いている状態で減速するときは0より大きくならない。

                // 状態を表す値の更新
                if (Mathf.Abs(_currentHorizontalSpeed) > 0.01f)
                {
                    _currentHorizontalMoveMode = HorizontalMoveMode.Deceleration;
                } // 速度が僅かでもある場合 「減速中」。
                else
                {
                    _currentHorizontalMoveMode = HorizontalMoveMode.Stop;
                } // 速度が限りなく0に近い時は 「停止」。
            }

            // 速度を割り当てる。
            _playerController.Rigidbody2D.velocity =
                    new Vector2(_currentHorizontalSpeed,
                    _playerController.Rigidbody2D.velocity.y);

            if (CanJump)
            {
                // ジャンプ処理
                if (_playerController.GroungChecker.IsHit(_playerController.DirectionControler.MovementDirectionX) &&
                    _playerController.InputManager.IsPressed[InputType.Jump])
                {
                    _playerController.Rigidbody2D.velocity =
                    new Vector2(
                        _playerController.Rigidbody2D.velocity.x,
                        _jumpPower);
                }
            }
        }

        public void EndOtherAction()
        {
            // 方向を表す値を更新する。（右に相当する値なら1, 左に相当する値なら-1。）
            _currentHorizontalSpeed = 0;
        }


        /// <summary>急停止する</summary>
        public void VelocityDeceleration()
        {
            _currentHorizontalSpeed = _playerController.Rigidbody2D.velocity.x;

            if (_playerController.GroungChecker.IsHit(_playerController.DirectionControler.MovementDirectionX))
            {
                _currentHorizontalSpeed -= Time.deltaTime * _landDeceleration * _playerController.Player.transform.localScale.x * GameManager.Instance.TimeController.PlayerTime;
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


        /// <summary>
        /// プレイヤーの水平方向移動状態を表現する値
        /// </summary>
        private enum HorizontalMoveMode
        {
            /// <summary> 停止状態 </summary>
            Stop,
            /// <summary> 入力発生直後 </summary>
            Start,
            /// <summary> 通常移動 </summary>
            Move,
            /// <summary> 減速 </summary>
            Deceleration,
        }
    }
}