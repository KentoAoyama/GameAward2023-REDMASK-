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

        [Header("=====カメラ1の設定=====")]
        [Header("左右のMaxOffSet")]
        [SerializeField] private float _maxOffsetX;

        [Header("上のMaxOffSet")]
        [SerializeField] private float _maxOffsetY;

        [Header("下のMaxOffSet")]
        [SerializeField] private float _underMaxOffsetY;

        [Header("左右のOffSetを変更する速度")]
        [SerializeField] private float _offsetMoveSpeedX;

        [Header("上下のOffSetを変更する速度")]
        [SerializeField] private float _offsetMoveSpeedY;

        [Header("=====カメラ2の設定=====")]

        [Header("カメラ追跡用のオブジェクト")]
        [SerializeField] private GameObject _target;

        [Header("左右どこまでカメラが行くか:初期値10")]
        [SerializeField] private float _maxMoveX = 10;

        [Header("上方向にどこまでカメラが行くか/初期値4")]
        [SerializeField] private float _maxMoveUpY = 4;

        [Header("下方向にどこまでカメラが行くか:初期値-4")]
        [SerializeField] private float _maxMoveDownY = -4;

        [Header("左右のカメラの移動速度:初期値30")]
        [SerializeField] private float _moveSpeedX = 30;

        [Header("上下のカメラの移動速度:初期値20")]
        [SerializeField] private float _moveSpeedY = 20;

        [SerializeField] private CinemachineVirtualCamera _camera1;
        [SerializeField] private CinemachineVirtualCamera _camera2;


        /// <summary>初期の位置</summary>
        private Vector2 _offsetStart;

        private float _beforX = 1;


        private PlayerController _playerController;

        private CinemachineTransposer _camera;

        enum CameraType
        {
            Camera1,
            Camera2,

        }

        public void Init(PlayerController playerController)
        {
            _playerController = playerController;
            _camera = _playerController.Camera.GetCinemachineComponent<CinemachineTransposer>();

            //nullチェック
            if (_camera == null || _camera1 == null || _camera2 == null) return;

            if (_cameraType == CameraType.Camera1)
            {
                _camera1.Priority = 10;
                _camera2.Priority = 0;
            }
            else
            {
                _camera1.Priority = 0;
                _camera2.Priority = 10;
            }

            _offsetStart = _camera.m_FollowOffset;

            _beforX = _playerController.Player.transform.localScale.x;
        }


        public void CameraLook()
        {
            Vector2 move = default;

            if (_beforX != _playerController.Player.transform.localScale.x)
            {
                _target.transform.localPosition = new Vector3(-_target.transform.localPosition.x, _target.transform.localPosition.y, _target.transform.localPosition.z);
            }

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


                    if (_playerController.Player.transform.localScale.x == 1)
                    {
                        if (_target.transform.localPosition.x < _maxMoveX)
                        {
                            float x = (_target.transform.localPosition.x + Time.deltaTime * _moveSpeedX);
                            _target.transform.localPosition = new Vector3(x, _target.transform.localPosition.y, _target.transform.localPosition.z);
                        }
                    }
                    else
                    {
                        if (_target.transform.localPosition.x > -_maxMoveX)
                        {
                            float x = (_target.transform.localPosition.x - Time.deltaTime * _moveSpeedX);
                            _target.transform.localPosition = new Vector3(x, _target.transform.localPosition.y, _target.transform.localPosition.z);
                        }
                    }



                }
                else if (move.x < -0.2f)
                {
                    if (_camera.m_FollowOffset.x > -_maxOffsetX)
                    {
                        _camera.m_FollowOffset.x -= _offsetMoveSpeedX;
                    }

                    if (_playerController.Player.transform.localScale.x == 1)
                    {
                        if (_target.transform.localPosition.x > -_maxMoveX)
                        {
                            float x = (_target.transform.localPosition.x - Time.deltaTime * _moveSpeedX);
                            _target.transform.localPosition = new Vector3(x, _target.transform.localPosition.y, _target.transform.localPosition.z);
                        }
                    }
                    else
                    {
                        if (_target.transform.localPosition.x < _maxMoveX)
                        {
                            float x = (_target.transform.localPosition.x + Time.deltaTime * _moveSpeedX);
                            _target.transform.localPosition = new Vector3(x, _target.transform.localPosition.y, _target.transform.localPosition.z);
                        }
                    }
                }
                else
                {
                    if (_target.transform.localPosition.x < -1)
                    {
                        float x = (_target.transform.localPosition.x + Time.deltaTime * _moveSpeedX);
                        _target.transform.localPosition = new Vector3(x, _target.transform.localPosition.y, _target.transform.localPosition.z);
                    }
                    if (_target.transform.localPosition.x > 1)
                    {
                        float x = (_target.transform.localPosition.x - Time.deltaTime * _moveSpeedX);
                        _target.transform.localPosition = new Vector3(x, _target.transform.localPosition.y, _target.transform.localPosition.z);
                    }
                }

                if (move.y > 0.1f)
                {
                    if (_camera.m_FollowOffset.y < _maxOffsetY)
                    {
                        _camera.m_FollowOffset.y += _offsetMoveSpeedY;
                    }

                    if (_target.transform.localPosition.y < _maxMoveUpY)
                    {
                        float y = _target.transform.localPosition.y + Time.deltaTime * _moveSpeedY;
                        _target.transform.localPosition = new Vector3(_target.transform.localPosition.x, y, _target.transform.localPosition.z);
                    }
                }
                else if (move.y < -0.1f)
                {
                    if (_camera.m_FollowOffset.y > _underMaxOffsetY)
                    {
                        _camera.m_FollowOffset.y -= _offsetMoveSpeedY;
                    }

                    if (_target.transform.localPosition.y > _maxMoveDownY)
                    {
                        float y = _target.transform.localPosition.y - Time.deltaTime * _moveSpeedY;
                        _target.transform.localPosition = new Vector3(_target.transform.localPosition.x, y, _target.transform.localPosition.z);
                    }
                }
                else
                {
                    if (_target.transform.localPosition.y < -1)
                    {
                        float y = _target.transform.localPosition.y + Time.deltaTime * _moveSpeedY;
                        _target.transform.localPosition = new Vector3(_target.transform.localPosition.x, y, _target.transform.localPosition.z);
                    }

                    if (_target.transform.localPosition.y > 1)
                    {
                        float y = _target.transform.localPosition.y - Time.deltaTime * _moveSpeedY;
                        _target.transform.localPosition = new Vector3(_target.transform.localPosition.x, y, _target.transform.localPosition.z);
                    }
                }
            }
            else
            {
                if (_offsetStart.x > _camera.m_FollowOffset.x)
                {
                    _camera.m_FollowOffset.x += _offsetMoveSpeedX;
                }
                else if (_offsetStart.x < _camera.m_FollowOffset.x)
                {
                    _camera.m_FollowOffset.x -= _offsetMoveSpeedX;
                }

                if (Mathf.Abs(_camera.m_FollowOffset.x - _offsetStart.x) < 0.1f)
                {
                    _camera.m_FollowOffset.x = _offsetStart.x;
                }



                if (_offsetStart.y > _camera.m_FollowOffset.y)
                {
                    _camera.m_FollowOffset.y += _offsetMoveSpeedY;
                }
                else if (_offsetStart.y < _camera.m_FollowOffset.y)
                {
                    _camera.m_FollowOffset.y -= _offsetMoveSpeedY;
                }

                if (Mathf.Abs(_camera.m_FollowOffset.y - _offsetStart.y) < 0.1f)
                {
                    _camera.m_FollowOffset.y = _offsetStart.y;
                }

            }
        }
    }
}