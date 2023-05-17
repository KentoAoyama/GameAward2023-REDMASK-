using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    [System.Serializable]
    public class PlayerBodyAndArmAngleSetting
    {
        [Header("======頭の設定======")]
        [Header("頭のSprite")]
        [Tooltip("頭のSprite"), SerializeField] private SpriteRenderer _headSprite;

        [Header("頭のイラスト、順番に入れてね")]
        [Tooltip("頭のイラスト、順番に入れてね"), SerializeField] private List<Sprite> _heads = new List<Sprite>();

        [Header("回避中の頭のイラスト")]
        [Tooltip("頭のイラスト、順番に入れてね"), SerializeField] private List<Sprite> _headsAvoid = new List<Sprite>();

        [SerializeField] private GameObject _headAvoid;

        [Header("======上半身の設定======")]

        [Header("上半身(前向き)の設定のSpriteRenderer")]
        [Tooltip("上半身(前向き)の設定のSprite"), SerializeField] private SpriteRenderer _upperBodySpriteRenderer;

        [Header("上半身(前向き)の設定のSprite")]
        [Tooltip("上半身(前向き)の設定のSprite"), SerializeField] private Sprite _upperBodySprite;

        [Header("上半身(後ろ向き)の設定のSprite")]
        [Tooltip("上半身(後ろ向き)の設定のSprite"), SerializeField] private Sprite _upperBodyBackSprite;


        [SerializeField] private GameObject _upperBodyAvoidRight;
        [SerializeField] private GameObject _upperBodyAvoidLeft;

        [Header("======下半身の設定======")]

        [Header("下半身(前向き)の設定のSpriteRenderer")]
        [Tooltip("下半身(前向き)の設定のSpriteRenderer"), SerializeField] private SpriteRenderer _downBodySpriteRenderer;

        [Header("下半身(前向き)の設定のSprite")]
        [Tooltip("下半身(前向き)の設定のSprite"), SerializeField] private Sprite _downBodySprite;

        [Header("下半身(後ろ向き)の設定のSprite")]
        [Tooltip("下半身(後ろ向き)の設定のSprite"), SerializeField] private Sprite _downBodyBackSprite;

        [Header("======腕の設定======")]
        [Header("通常の腕のオブジェクト")]
        [Tooltip("通常の腕のオブジェクト"), SerializeField] private GameObject _arm;

        [Header("腕のSprite")]
        [Tooltip("腕のSprite"), SerializeField] private SpriteRenderer _armSprite;

        [Header("腕、順番に入れてね")]
        [Tooltip("腕、順番に入れてね"), SerializeField] private List<GameObject> arms = new List<GameObject>();

        [Header("腕、順番に入れてね")]
        [Tooltip("腕、順番に入れてね"), SerializeField] private List<GameObject> _armsAavoid = new List<GameObject>();

        private int _animationScaleX = 1;


        private int _nowMuzzleNum;




        private float _angleRight = default;

        private float _angleLeft = default;

        private float _imageAngleRight = default;
        private float _imageAngleLeft = default;

        private Vector3 _currentMousePos;

        private PlayerController _playerController;

        public int AnimationScaleX => _animationScaleX;
        public Transform ArmCenterPos => _arm.transform;

        public int MuzzleNum => _nowMuzzleNum;

        public GameObject Arm => _arm;

        public void Init(PlayerController playerController)
        {
            _playerController = playerController;

            Vector2ToAngle(_playerController.Move.MoveHorizontalDir, Vector2.right, false);

            //真上を基準とした角度を取得
            float angle = Vector2ToAngle(_playerController.Move.MoveHorizontalDir, Vector2.right, true);

            var i = Mathf.Abs((int)Mathf.Floor(angle / 30));

            arms.ForEach(i => i.SetActive(false));
            arms[i].SetActive(true);
            _nowMuzzleNum = i;

            _currentMousePos = _playerController.InputManager.GetValue<Vector2>(InputType.LookingMausePos);
        }

        /// <summary>ベクトルから角度を求める</summary>
        /// <param name="vector">ベクトルの向き</param>
        /// <returns>基準(0度)を真上に、かつ、時計回りに度数をふやすかどうか</returns>
        private float Vector2ToAngle(float moveH, Vector2 vector, bool isChangeStandard)
        {
            //視点を動かしていない場合は、前回決めた値を変えす
            if (vector == Vector2.zero)
            {
                if (isChangeStandard)
                {
                    if (moveH < 0) return _imageAngleLeft;
                    else return _imageAngleRight;
                }   //イラスト用の角度
                else
                {
                    if (moveH < 0) return _angleLeft;
                    else return _angleRight;
                }   //腕の回転の角度
            }

            // ラジアンから度数に変換
            float angleRight = Mathf.Atan2(vector.y, vector.x) * Mathf.Rad2Deg;
            float angleLeft = Mathf.Atan2(-vector.y, -vector.x) * Mathf.Rad2Deg;

            //イラスト用の角度
            if (isChangeStandard)
            {
                //左向きの時の回転を設定
                angleLeft = (angleLeft + 360f) % 360f; // 0～360度の範囲に調整
                angleLeft = (angleLeft + 90f) % 360f; // 90度回転
                _imageAngleLeft = angleLeft;

                //右向きの時の回転を設定
                angleRight = 90 - angleRight; // 90度回転
                if (angleRight < 0) angleRight += 360; // 0～360度の範囲に調整
                _imageAngleRight = angleRight;

                if (moveH < 0) return _imageAngleLeft;
                else return _imageAngleRight;

            }   //腕の回転の角度
            else
            {
                _angleLeft = angleLeft;
                _angleRight = angleRight;

                if (moveH < 0) return _angleLeft;
                else return _angleRight;
            }
        }

        public void Update()
        {
            if (GameManager.Instance.PauseManager.PauseCounter > 0)
            {
                return;
            } // ポーズ中は何もできない


            if (!_playerController.GunSetUp.IsGunSetUp || _playerController.PlayerAnimatorControl.IsAnimationNow)
            {
                return;
            }



            Vector2 _aimingAngle = default;

            // 撃つ方向を保存する
            if (_playerController.DeviceManager.CurrentDevice.Value == Input.Device.GamePad) // ゲームパッド操作の場合
            {
                if ((_playerController.InputManager.GetValue<Vector2>(InputType.LookingAngleGamePad)).magnitude > 0.5f)
                {
                    _aimingAngle = _playerController.InputManager.GetValue<Vector2>(InputType.LookingAngleGamePad);
                    _playerController.RevolverOperator.StopRevolverReLoad();
                    _playerController.Revolver.OffDrawAimingLine(true);
                }
            }
            else // マウス操作の場合
            {
                // マウスの座標をワールド座標に変換する
                Vector3 mouseDir = _playerController.InputManager.GetValue<Vector2>(InputType.LookingMausePos);
                mouseDir.z = 10f;
                var mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseDir);
                _aimingAngle = mouseWorldPos - _arm.transform.position;


                float distance = Vector2.Distance(mouseDir, _currentMousePos);
                _currentMousePos = mouseDir;
                if (distance > 3f)
                {
                    _playerController.RevolverOperator.StopRevolverReLoad();
                    _playerController.Revolver.OffDrawAimingLine(true);
                }
            }

            if(_aimingAngle.x>=0)
            {
                _animationScaleX = 1;
            }
            else
            {
                _animationScaleX = -1;
            }

            //360度の角度を取得
            float armAngle = Vector2ToAngle(_playerController.Move.MoveHorizontalDir, _aimingAngle, false);

            if (!_playerController.Avoidance.IsAvoidanceNow)
            {
                //腕の角度を変更
                _arm.transform.rotation = Quaternion.Euler(0f, 0f, armAngle);

                //真上を基準とした角度を取得
                float angle = Vector2ToAngle(_playerController.Move.MoveHorizontalDir, _aimingAngle, true);
                var i = Mathf.Abs((int)Mathf.Floor(angle / 30));

                //頭のSpriteを変更        
                _headSprite.sprite = _heads[i];
                //腕のを変更
                arms.ForEach(i => i.SetActive(false));
                arms[i].SetActive(true);

                _nowMuzzleNum = i;


                if (_playerController.Move.MoveHorizontalDir > 0)
                {
                    if (0 <= angle && angle < 180)
                    {
                        _upperBodySpriteRenderer.sprite = _upperBodySprite;
                        _downBodySpriteRenderer.sprite = _downBodySprite;
                    }
                    else
                    {
                        _upperBodySpriteRenderer.sprite = _upperBodyBackSprite;
                        _downBodySpriteRenderer.sprite = _downBodyBackSprite;
                    }
                }
                else
                {
                    if (0 <= angle && angle < 180)
                    {
                        _upperBodySpriteRenderer.sprite = _upperBodyBackSprite;
                        _downBodySpriteRenderer.sprite = _downBodyBackSprite;
                    }
                    else
                    {
                        _upperBodySpriteRenderer.sprite = _upperBodySprite;
                        _downBodySpriteRenderer.sprite = _downBodySprite;
                    }
                }
            }
            else
            {
                //360度の角度を取得
                armAngle = Vector2ToAngle(_playerController.Move.MoveHorizontalDir, _aimingAngle, false);

                //腕の角度を変更
                _arm.transform.rotation = Quaternion.Euler(0f, 0f, armAngle);

                //真上を基準とした角度を取得
                float angle = Vector2ToAngle(_playerController.Move.MoveHorizontalDir, _aimingAngle, true);



                if (angle >= 0 && angle <= 180)
                {
                    _upperBodyAvoidRight.SetActive(true);
                    _upperBodyAvoidLeft.SetActive(false);
                }
                else
                {
                    _upperBodyAvoidRight.SetActive(false);
                    _upperBodyAvoidLeft.SetActive(true);
                }

                var i = Mathf.Abs((int)Mathf.Floor(angle / 30));

                //頭のSpriteを変更        
                _headSprite.sprite = _headsAvoid[0];
                //腕のを変更
                _armsAavoid.ForEach(i => i.SetActive(false));
                _armsAavoid[i].SetActive(true);

                _nowMuzzleNum = i;
            }
        }
    }
}