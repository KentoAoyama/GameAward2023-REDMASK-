using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

/// <summary>�J�����̐���</summary>
namespace Player
{
    [System.Serializable]

    public class CameraShake
    {
        private PlayerController _playerController;

        private CinemachineImpulseSource _source;

        public void Init(PlayerController playerController)
        {
            _playerController = playerController;
            _source = _playerController.Camera.GetComponent<CinemachineImpulseSource>();
        }


        public void RevolverShootShake()
        {
            _source.GenerateImpulse();
            Debug.Log("�U��");
        }
    }
}
