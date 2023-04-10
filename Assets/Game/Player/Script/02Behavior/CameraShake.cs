using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Cysharp.Threading.Tasks;

/// <summary>�J�����̐���</summary>
namespace Player
{
    [System.Serializable]

    public class CameraShake
    {
        [Header("���S���ɃJ�����̗h�炷���ǂ���")]
        [Tooltip("�v���C���[�̃I�u�W�F�N�g"), SerializeField]
        private bool _isDeadCameraChake = false;

        private PlayerController _playerController;

        private CinemachineImpulseSource _source;
        CinemachineImpulseListener impulseListener;

        public void Init(PlayerController playerController)
        {
            _playerController = playerController;
            _source = _playerController.Camera.GetComponent<CinemachineImpulseSource>();
            impulseListener = _playerController.Camera.GetComponent<CinemachineImpulseListener>();
        }

        public void SlowCamera()
        {

        }


        public void RevolverShootShake()
        {
            _source.GenerateImpulse();
        }

        public void DeadCameraShake()
        {
            if (_isDeadCameraChake)
            {
                //�J������h�炷
                _source.GenerateImpulse();
            }
            else
            {
                //�J�����̗h����~�߂�
                impulseListener.enabled = false;
            }
        }

        public async void Pause()
        {
            await UniTask.WaitUntil(() => _playerController != null);
            Debug.Log("�J�����̐U�����X�g�b�v");
            impulseListener.enabled = false;
        }
        public void Resume()
        {
            Debug.Log("�J�����̐U�����Đ�");
            impulseListener.enabled = true;
        }

    }
}
