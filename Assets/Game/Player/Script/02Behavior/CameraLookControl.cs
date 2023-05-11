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

        [Header("�����lOffSet")]
        private Vector2 _offsetStart;

        private PlayerController _playerController;

        private CinemachineTransposer _camera;

        public void Init(PlayerController playerController)
        {
            _playerController = playerController;
            _camera = _playerController.Camera.GetCinemachineComponent<CinemachineTransposer>();

            _offsetStart = _camera.m_FollowOffset;
        }


        public void CameraLook()
        {
            Vector2 move = default;

            if (_playerController.GunSetUp.IsGunSetUp)
            {
                // ��������ۑ�����
                if (_playerController.DeviceManager.CurrentDevice.Value == Input.Device.GamePad) // �Q�[���p�b�h����̏ꍇ
                {
                    //if ((_playerController.InputManager.GetValue<Vector2>(InputType.LookingAngleGamePad)).magnitude > 0.5f)
                    //{
                    move = _playerController.InputManager.GetValue<Vector2>(InputType.LookingAngleGamePad);
                    //}
                }
                else // �}�E�X����̏ꍇ
                {
                    // �}�E�X�̍��W�����[���h���W�ɕϊ�����
                    Vector3 mouseDir = _playerController.InputManager.GetValue<Vector2>(InputType.LookingMausePos);
                    mouseDir.z = 10f;
                    var mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseDir);

                    Vector2 centerPosition = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width / 2f, Screen.height / 2f));

                    //Vector2 centerPosition = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width / 2f, Screen.height / 2f));

                    //move = (Vector2)mouseWorldPos - centerPosition;

                    move = mouseWorldPos - _playerController.BodyAnglSetteing.Arm.transform.position;
                }

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

                if (move.y > 0.2f)
                {
                    if (_camera.m_FollowOffset.y < _maxOffsetY)
                    {
                        _camera.m_FollowOffset.y += _offsetMoveSpeedY;
                    }
                }
                else if (move.y < -0.2f)
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