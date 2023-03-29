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
        private PlayerController _playerController;

        private CinemachineImpulseSource _source;
        CinemachineImpulseListener impulseListener;

        Vector3 saveVelo;

        public void Init(PlayerController playerController)
        {
            _playerController = playerController;
            _source = _playerController.Camera.GetComponent<CinemachineImpulseSource>();
            impulseListener = _playerController.Camera.GetComponent<CinemachineImpulseListener>();
        }


        public void RevolverShootShake()
        {
            _source.GenerateImpulse();

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
