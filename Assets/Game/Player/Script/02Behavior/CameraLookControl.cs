using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Cysharp.Threading.Tasks;

/// <summary>�J�����̐���</summary>
namespace Player
{
    [System.Serializable]
    public class CameraLookControl
    {
        private CameraType _cameraType = CameraType.Camera1;

        [Header("=====�J����1�̐ݒ�=====")]
        [Header("���E��MaxOffSet")]
        [SerializeField] private float _maxOffsetX;

        [Header("���MaxOffSet")]
        [SerializeField] private float _maxOffsetY;

        [Header("����MaxOffSet")]
        [SerializeField] private float _underMaxOffsetY;

        [Header("���E��OffSet��ύX���鑬�x")]
        [SerializeField] private float _offsetMoveSpeedX;

        [Header("�㉺��OffSet��ύX���鑬�x")]
        [SerializeField] private float _offsetMoveSpeedY;

        [Header("=====�J����2�̐ݒ�=====")]

        [Header("�J�����ǐ՗p�̃I�u�W�F�N�g")]
        [SerializeField] private GameObject _target;

        [Header("���E�ǂ��܂ŃJ�������s����:�����l10")]
        [SerializeField] private float _maxMoveX = 10;

        [Header("������ɂǂ��܂ŃJ�������s����/�����l4")]
        [SerializeField] private float _maxMoveUpY = 4;

        [Header("�������ɂǂ��܂ŃJ�������s����:�����l-4")]
        [SerializeField] private float _maxMoveDownY = -4;

        [Header("���E�̃J�����̈ړ����x:�����l30")]
        [SerializeField] private float _moveSpeedX = 30;

        [Header("�㉺�̃J�����̈ړ����x:�����l20")]
        [SerializeField] private float _moveSpeedY = 20;

        [SerializeField] private CinemachineVirtualCamera _camera1;
        [SerializeField] private CinemachineVirtualCamera _camera2;


        /// <summary>�����̈ʒu</summary>
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

            //null�`�F�b�N
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

            // ��������ۑ�����
            if (_playerController.DeviceManager.CurrentDevice.Value == Input.Device.GamePad) // �Q�[���p�b�h����̏ꍇ
            {
                move = _playerController.InputManager.GetValue<Vector2>(InputType.LookingAngleGamePad);
            }
            else // �}�E�X����̏ꍇ
            {
                // �}�E�X�̍��W�����[���h���W�ɕϊ�����
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