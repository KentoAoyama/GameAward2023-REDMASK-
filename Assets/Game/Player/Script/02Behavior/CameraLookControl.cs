using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Cysharp.Threading.Tasks;

/// <summary>カメラの制御</summary>
namespace Player
{
    [System.Serializable]
    public class CameraLookControl
    {
        private CameraType _cameraType = CameraType.Camera1;

        // [Header("=====カメラ1の設定=====")]
        [Header("左右のカメラ範囲")]
        [SerializeField] [Range(0, 10f)] private float _maxOffsetX;

        [Header("上のカメラ範囲")]
        [SerializeField] [Range(0, 7f)] private float _maxOffsetY;

        [Header("下のカメラ範囲")]
        [SerializeField] [Range(0, 7f)] private float _underMaxOffsetY;

        [Header("左右のカメラ移動速度")]
        [SerializeField] [Range(0, 0.15f)] private float _offsetMoveSpeedX;

        [Header("上下のカメラ移動速度")]
        [SerializeField] [Range(0, 0.15f)] private float _offsetMoveSpeedY;

        [SerializeField] private CinemachineVirtualCamera _camera1;

        /// <summary>初期の位置</summary>
        [SerializeField] private Vector2 _offsetStart;

        private float _beforX = 1;


        private PlayerController _playerController;

        private CinemachineTransposer _camera;

        enum CameraType
        {
            Camera1,
        }

        public void Init(PlayerController playerController)
        {
            _playerController = playerController;
            _camera = _playerController.Camera.GetCinemachineComponent<CinemachineTransposer>();

            //nullチェック
            if (_camera == null || _camera1 == null) return;

            _beforX = _playerController.Player.transform.localScale.x;
        }


        public void CameraLook()
        {
            Vector2 move = default;

            _beforX = _playerController.Player.transform.localScale.x;

            // 撃つ方向を保存する
            if (_playerController.DeviceManager.CurrentDevice.Value == Input.Device.GamePad) // ゲームパッド操作の場合
            {
                move = _playerController.InputManager.GetValue<Vector2>(InputType.LookingAngleGamePad);
            }
            else // マウス操作の場合
            {
                // マウスの座標をワールド座標に変換する
                Vector3 mouseDir = _playerController.InputManager.GetValue<Vector2>(InputType.LookingMausePos);
                mouseDir.z = 10f;
                var mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseDir);

                Vector2 centerPosition = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width / 2f, Screen.height / 2f));


                move = (Vector2)mouseWorldPos - (Vector2)_playerController.Player.transform.position;

                //   move = mouseWorldPos - _playerController.BodyAnglSetteing.Arm.transform.position;
            }

            if (move != Vector2.zero)
            {

                if (move.x > 0.2f)
                {

                    if (_camera.m_FollowOffset.x < _maxOffsetX)
                    {
                        _camera.m_FollowOffset.x += _offsetMoveSpeedX;
                    }
                }
                else if (move.x < -0.2f)
                {
                    if (_camera.m_FollowOffset.x > -_maxOffsetX)
                    {
                        _camera.m_FollowOffset.x -= _offsetMoveSpeedX;
                    }
                }
                else
                {

                }

                if (move.y > 0.1f)
                {
                    if (_camera.m_FollowOffset.y < _maxOffsetY)
                    {
                        _camera.m_FollowOffset.y += _offsetMoveSpeedY;
                    }

                }
                else if (move.y < -0.1f)
                {
                    if (_camera.m_FollowOffset.y > _underMaxOffsetY)
                    {
                        _camera.m_FollowOffset.y -= _offsetMoveSpeedY;
                    }

                }
                else
                {

                }
            }
            else
            {
                //if (_offsetStart.x > _camera.m_FollowOffset.x)
                //{
                //    _camera.m_FollowOffset.x += _offsetMoveSpeedX;
                //}
                //else if (_offsetStart.x < _camera.m_FollowOffset.x)
                //{
                //    _camera.m_FollowOffset.x -= _offsetMoveSpeedX;
                //}

                //if (Mathf.Abs(_camera.m_FollowOffset.x - _offsetStart.x) < 0.1f)
                //{
                //    _camera.m_FollowOffset.x = _offsetStart.x;
                //}



                //if (_offsetStart.y > _camera.m_FollowOffset.y)
                //{
                //    _camera.m_FollowOffset.y += _offsetMoveSpeedY;
                //}
                //else if (_offsetStart.y < _camera.m_FollowOffset.y)
                //{
                //    _camera.m_FollowOffset.y -= _offsetMoveSpeedY;
                //}

                //if (Mathf.Abs(_camera.m_FollowOffset.y - _offsetStart.y) < 0.1f)
                //{
                //    _camera.m_FollowOffset.y = _offsetStart.y;
                //}

            }
        }
    }
}