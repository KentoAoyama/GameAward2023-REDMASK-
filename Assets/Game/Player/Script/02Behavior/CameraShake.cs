using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Cysharp.Threading.Tasks;

/// <summary>カメラの制御</summary>
namespace Player
{
    [System.Serializable]

    public class CameraShake
    {
        [Header("死亡事にカメラの揺らすかどうか")]
        [Tooltip("プレイヤーのオブジェクト"), SerializeField]
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
                //カメラを揺らす
                _source.GenerateImpulse();
            }
            else
            {
                //カメラの揺れを止める
                impulseListener.enabled = false;
            }
        }

        public async void Pause()
        {
            await UniTask.WaitUntil(() => _playerController != null);
            Debug.Log("カメラの振動をストップ");
            impulseListener.enabled = false;
        }
        public void Resume()
        {
            Debug.Log("カメラの振動を再生");
            impulseListener.enabled = true;
        }

    }
}
