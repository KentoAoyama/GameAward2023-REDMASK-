using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    [System.Serializable]
    public class PlayerBodyAndArmAngleSetting
    {
        [Header("腕のオブジェクト")]
        [Tooltip("腕のオブジェクト"), SerializeField] private GameObject _arm;

        [Header("腕のSprite")]
        [Tooltip("腕のSprite"), SerializeField] private SpriteRenderer _armSprite;

        [Header("腕のイラスト、順番に入れてね")]
        [Tooltip("腕のイラスト、順番に入れてね"), SerializeField] private List<Sprite> arms = new List<Sprite>();

        private float _angleRight = default;

        private float _angleLeft = default;

        private float _imageAngleRight = default;
        private float _imageAngleLeft = default;

        private PlayerController _playerController;

        public void Init(PlayerController playerController)
        {
            _playerController = playerController;

            Vector2ToAngle(_playerController.Move.MoveHorizontalDir, Vector2.right, false);

            //真上を基準とした角度を取得
            float angle = Vector2ToAngle(_playerController.Move.MoveHorizontalDir, Vector2.right, true);

            var i = Mathf.Abs((int)Mathf.Floor(angle / 30));

            //Spriteを変更
            _armSprite.sprite = arms[i];
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
            Vector2 _aimingAngle = default;

            // 撃つ方向を保存する
            if (_playerController.DeviceManager.CurrentDevice.Value == Input.Device.GamePad) // ゲームパッド操作の場合
            {
                if ((_playerController.InputManager.GetValue<Vector2>(InputType.LookingAngleGamePad)).magnitude > 0.5f)
                {
                    _aimingAngle = _playerController.InputManager.GetValue<Vector2>(InputType.LookingAngleGamePad);
                }
            }
            else // マウス操作の場合
            {
                // マウスの座標をワールド座標に変換する
                var mouseWorldPos = Camera.main.ScreenToWorldPoint(
                    _playerController.InputManager.GetValue<Vector2>(InputType.LookingMausePos));
                if (((Vector2)mouseWorldPos - (Vector2)_playerController.transform.position).sqrMagnitude > 0.5f)
                {


                    _aimingAngle = mouseWorldPos - _playerController.transform.position;
                }
            }

            //腕の角度を取得
            float armAngle = Vector2ToAngle(_playerController.Move.MoveHorizontalDir, _aimingAngle, false);

            //腕の角度を変更
            _arm.transform.rotation = Quaternion.Euler(0f, 0f, armAngle);


            //真上を基準とした角度を取得
            float angle = Vector2ToAngle(_playerController.Move.MoveHorizontalDir, _aimingAngle, true);

            var i = Mathf.Abs((int)Mathf.Floor(angle / 30));

            //Spriteを変更
            _armSprite.sprite = arms[i];
        }
    }
}