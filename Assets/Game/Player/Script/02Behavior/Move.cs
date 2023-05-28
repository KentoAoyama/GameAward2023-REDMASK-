using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    [System.Serializable]
    public class Move
    {

        [Header("===坂道検出の設定===")]

        [Header("坂の許容値")]
        [SerializeField] [Range(0, 0.2f)] private float _sakaYurusu = 0.1f;

        [Header("地面のレイヤー")]
        [SerializeField] private LayerMask _ground;
        [Header("地面を検知するRayの長さ")]
        [SerializeField] private float _rayLong = 1.2f;
        [Header("前後、真下のRayのOffSet")]
        [SerializeField] private float _rayOffSetX = 0.2f;
        [Header("足元化から前方向のRayの長さ")]
        [SerializeField] private float _frontRayLong = 1.2f;
        [Header("足元化から前方向のRayのOffSet")]
        [SerializeField] private Vector2 _frontRayOffSetX = default;

        [Header("前後、真下のRayのOffSet")]
        [SerializeField] private float _midleRayOffSetX = 0.1f;

        [Header("右側斜めのRayのOffSet")]
        [SerializeField] private Vector2 _crossRightRayOffSetX = default;

        [Header("左側斜めのRayのOffSet")]
        [SerializeField] private Vector2 _crossLeftRayOffSetX = default;

        [Header("===移動の設定===")]
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

        Vector2 dir;

        Vector2 groundDir;

        private bool _isJump;

        private bool _isSound = false;

        private int _moveSoundIndex = -1;

        private PlayerController _playerController;

        public int MoveSoundIndex => _moveSoundIndex;

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

        public void StopMoveSE()
        {
            _isSound = false;
            GameManager.Instance.AudioManager.StopSE(_moveSoundIndex);
            _moveSoundIndex = -1;
        }

        public bool CheckMoveDir(float inputH)
        {
            //足元から正面にRayを飛ばす
            RaycastHit2D frontRay = Physics2D.Raycast((Vector2)_playerController.Player.transform.position + _frontRayOffSetX,
                                     Vector2.right,
                                    _frontRayLong, _ground);

            RaycastHit2D frontRayL = Physics2D.Raycast((Vector2)_playerController.Player.transform.position + _frontRayOffSetX,
                         Vector2.left,
                        _frontRayLong, _ground);


            //プレイヤーから、下方向(手前)にRayを打つ
            RaycastHit2D hitPluseX = Physics2D.Raycast(new Vector2(_rayOffSetX + _playerController.Player.transform.position.x, _playerController.Player.transform.position.y),
                                    Vector2.down,
                                    _rayLong,
                                    _ground);


            //プレイヤーから、下方向(後ろ)にRayを打つ
            RaycastHit2D hitMinusX = Physics2D.Raycast(new Vector2(-_rayOffSetX + _playerController.Player.transform.position.x, _playerController.Player.transform.position.y),
                                    Vector2.down,
                                    _rayLong,
                                    _ground);

            Vector2 moveDir = Vector2.right * inputH;

            float angle = 0;


            Vector2 dirR = (Vector2)_playerController.Player.transform.position + _crossRightRayOffSetX - (Vector2)_playerController.Player.transform.position;
            Vector2 dirL = (Vector2)_playerController.Player.transform.position + _crossLeftRayOffSetX - (Vector2)_playerController.Player.transform.position;


            RaycastHit2D hitCrossR = Physics2D.Raycast(_playerController.Player.transform.position,
                               dirR,
                               _rayLong,
                               _ground);

            RaycastHit2D hitCrossL = Physics2D.Raycast(_playerController.Player.transform.position,
                   dirL,
                   _rayLong,
                   _ground);

            Debug.DrawRay(_playerController.Player.transform.position, dirL * _rayLong, Color.red);
            Debug.DrawRay(_playerController.Player.transform.position, dirR * _rayLong, Color.red);


            if ((hitCrossR.normal.x < _sakaYurusu && hitCrossR.normal.x > -_sakaYurusu) && (hitCrossL.normal.x < _sakaYurusu && hitCrossL.normal.x > -_sakaYurusu))
            {
                Debug.Log("ON");
                //_playerController.Rigidbody2D.gravityScale = 1f;
                return false;
            }
            else
            {
                Debug.Log("OFF");
                // _playerController.Rigidbody2D.gravityScale = 0f;
            }


            if (_playerController.Player.transform.localScale.x == 1)
            {
                //右側の坂を登り終えそうなとき
                //"右側の法線が真上" && "左側のRayが当たってない"　|| "左側の法線が左向き"
                if ((hitCrossR.normal.x < _sakaYurusu && hitCrossR.normal.x > -_sakaYurusu) && (!hitCrossL || hitCrossL.normal.x < -_sakaYurusu))
                {
                    Debug.Log($"右側の坂を登り終えそうなとき");
                    return false;
                }



                //右上向きの坂を登っている
                if (hitCrossR.normal.x < -_sakaYurusu)
                {
                    //真上と、法線の角度を求める
                    angle = Vector3.Angle(Vector2.up, hitCrossR.normal);

                    if (hitCrossR.normal.x > 0)
                    {
                        angle = 360 - angle;
                    }   //左上に進む坂に対応させる

                    dir = Quaternion.AngleAxis(angle, Vector3.forward) * moveDir;

                    Debug.Log($"右上向きの坂を登っている");
                    return true;
                }

                //右下向きの坂を下っている。
                if (hitCrossL.normal.x > _sakaYurusu)
                {
                    Debug.Log("右下向きの坂を下っている");
                    //真上と、法線の角度を求める
                    angle = Vector3.Angle(Vector2.up, hitCrossL.normal);

                    if (hitCrossL.normal.x > 0)
                    {
                        angle = 360 - angle;
                    }   //左上に進む坂に対応させる

                    dir = Quaternion.AngleAxis(angle, Vector3.forward) * moveDir;
                    return true;
                }

                //左側の坂を下り終えそうなとき
                //"右側の法線が真上" && "左側のRayが当たっている"
                if ((hitCrossR.normal.x < _sakaYurusu && hitCrossR.normal.x > -_sakaYurusu) && hitCrossL.normal.x > _sakaYurusu)
                {
                    //真上と、法線の角度を求める
                    angle = Vector3.Angle(Vector2.up, hitCrossL.normal);

                    if (hitCrossL.normal.x > 0)
                    {
                        angle = 360 - angle;
                    }   //左上に進む坂に対応させる

                    dir = Quaternion.AngleAxis(angle, Vector3.forward) * moveDir;
                    return true;
                }


            }
            else　//プレイヤーが左向きの時
            {

                //左側の坂を登り終えそうなとき
                //"左側の法線が真上" && "右側のRayが当たってない"　|| "右側の法線が左向き"
                if (hitCrossL.normal.x == 0 && (!hitCrossR || hitCrossR.normal.x > _sakaYurusu))
                {
                    return false;
                }



                //左側の坂を登っている
                if (hitCrossL.normal.x > _sakaYurusu)
                {
                    //真上と、法線の角度を求める
                    angle = Vector3.Angle(Vector2.up, hitCrossL.normal);

                    if (hitCrossL.normal.x > 0)
                    {
                        angle = 360 - angle;
                    }   //左上に進む坂に対応させる

                    dir = Quaternion.AngleAxis(angle, Vector3.forward) * moveDir;
                    return true;
                }

                //右側を下っている
                if (hitCrossR.normal.x < -_sakaYurusu)
                {
                    //真上と、法線の角度を求める
                    angle = Vector3.Angle(Vector2.up, hitCrossR.normal);

                    if (hitCrossR.normal.x > 0)
                    {
                        angle = 360 - angle;
                    }   //左上に進む坂に対応させる

                    dir = Quaternion.AngleAxis(angle, Vector3.forward) * moveDir;
                    return true;
                }

                //右側の坂を下り終えそうなとき
                //"左側の法線が真上" && "右側のRayが当たっている"
                if ((hitCrossL.normal.x < _sakaYurusu && hitCrossL.normal.x > -_sakaYurusu) && hitCrossR.normal.x < -_sakaYurusu)
                {
                    //真上と、法線の角度を求める
                    angle = Vector3.Angle(Vector2.up, hitCrossL.normal);

                    if (hitCrossL.normal.x > 0)
                    {
                        angle = 360 - angle;
                    }   //左上に進む坂に対応させる

                    dir = Quaternion.AngleAxis(angle, Vector3.forward) * moveDir;
                    return true;
                }


            }

            return false;
        }

        public void Update()
        {
            if (_playerController.IsDead)
            {
                return;
            }

            //ポーズ中、回避中、近接攻撃中、構え中、発砲中
            //に移動不可
            if (IsPause || _playerController.Avoidance.IsAvoidanceNow || _playerController.Proximity.IsProximityNow
                || _playerController.GunSetUp.IsGunSetUp || _playerController.RevolverOperator.IsFireNow)
            {
                if (_isSound)
                {
                    StopMoveSE();
                }
                return;
            }

            var gameTime = GameManager.Instance.TimeController.PlayerTime;

            // 移動入力があるときの処理
            if (_playerController.InputManager.IsExist[InputType.MoveHorizontal] && CanMove)
            {

                if (!_isSound && !_isJump)
                {
                    _isSound = true;
                    _moveSoundIndex = GameManager.Instance.AudioManager.PlaySE("CueSheet_Gun", "SE_Player_Run");
                }

                // 1フレーム前の水平移動方向（入力方向）を保存しておく
                _previousDir = _moveHorizontalDir;
                // 方向を表す値を更新する。（右に相当する値なら1, 左に相当する値なら-1。）
                _moveHorizontalDir = _playerController.InputManager.GetValue<float>(InputType.MoveHorizontal) > 0f ? 1f : -1f;

                //止まっている状態から動き始めたら、リロード中断処理を実行
                if (_previousDir == 0 || _moveHorizontalDir != 0)
                {
                    _playerController.RevolverOperator.StopRevolverReLoad();
                }

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
                    else if (_currentHorizontalSpeed < -_maxMidairMoveSpeed) _currentHorizontalSpeed = -_maxMidairMoveSpeed;

                }
            }
            // 入力がないとき、現在速度を減算する。
            else
            {
                if (_isSound)
                {
                    StopMoveSE();
                }

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

            //地面についているときは、床との外積を考慮する
            if (_playerController.GroungChecker.IsHit(_playerController.DirectionControler.MovementDirectionX))
            {
                if (_playerController.InputManager.IsExist[InputType.MoveHorizontal])
                {
                    _playerController.Rigidbody2D.gravityScale = 1f;
                }
                else
                {
                    _playerController.Rigidbody2D.gravityScale = 0f;
                }

                if (CheckMoveDir(_moveHorizontalDir))
                {
                    _playerController.Rigidbody2D.velocity = Mathf.Abs(_currentHorizontalSpeed) * dir;
                }
                else
                {
                    dir = Vector2.right * _moveHorizontalDir;

                    _playerController.Rigidbody2D.velocity =
                            new Vector2(_currentHorizontalSpeed,
                           -1);
                }
            }
            else
            {
                _playerController.Rigidbody2D.gravityScale = 1f;
                _playerController.Rigidbody2D.velocity =
                        new Vector2(_currentHorizontalSpeed,
                       _playerController.Rigidbody2D.velocity.y);
            }



            Debug.DrawRay(_playerController.Player.transform.position, dir * _rayLong, Color.red);
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
            if (CheckMoveDir(_previousDir))
            {
                _playerController.Rigidbody2D.velocity = _currentHorizontalSpeed * dir;
            }
            else
            {
                _playerController.Rigidbody2D.velocity =
                        new Vector2(_currentHorizontalSpeed,
                       -1);
            }

            if (_playerController.GunSetUp.IsGunSetUp)
            {
                if (_playerController.GroungChecker.IsHit(_playerController.DirectionControler.MovementDirectionX))
                {
                    Vector2 velo = new Vector2(_playerController.Rigidbody2D.velocity.x, 0);
                    _playerController.Rigidbody2D.velocity = velo;
                }
                else
                {
                    Vector2 velo = new Vector2(_playerController.Rigidbody2D.velocity.x, -1);
                    _playerController.Rigidbody2D.velocity = velo;
                }

            }

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