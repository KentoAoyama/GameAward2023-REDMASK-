using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

namespace Player
{
    [System.Serializable]
    public class GunSetUp
    {
        [Header("�\����܂ł̎���(�b)")]
        [SerializeField] private float _setUpTime = 0.2f;

        [Header("Test�p�A���x����Text")]
        [SerializeField]
        private GameObject _testSlowTimeText;

        private float _setUpTimeCount = 0;

        private bool _isSlowTimeNow = false;

        private int _beatSoundIndex = -1;

        public bool IsSlowTimeNow => _isSlowTimeNow;

        private PlayerController _playerController = null;

        private bool _isEmergencyStopSlowTime = false;

        /// <summary>�\���Ă��邩�ǂ���</summary>
        private bool _isGunSetUp = false;

        private bool _isReserveSetUpAvoid = false;

        public bool IsGunSetUp => _isGunSetUp;

        public bool IsPause { get; private set; } = false;

        public async void Pause()
        {
            await UniTask.WaitUntil(() => _playerController != null);
            // Update���~����
            IsPause = true;
        }
        public void Resume()
        {
            // Update���ĊJ����
            IsPause = false;

            AnimEndSetUpCheck();
        }

        public void Init(PlayerController playerController)
        {
            _playerController = playerController;
        }


        public void AnimEndSetUpCheck()
        {
            _isEmergencyStopSlowTime = false;

            if (_playerController.InputManager.IsExist[InputType.GunSetUp])
            {
                _isGunSetUp = true;

                if (!_isSlowTimeNow)
                {
                    DoSlow();
                }

                //�r���o��
                _playerController.PlayerAnimatorControl.GunSet(true);

                //�r�̊p�x��ݒ�
                _playerController.BodyAnglSetteing.AimingSet();

                //�Ə���`��
                _playerController.Revolver.OnDrawAimingLine();
            }
            else
            {
                EndSlowTime();

                //�d�͂�߂�
                _playerController.Rigidbody2D.gravityScale = 1f;

                _isGunSetUp = false;

                _playerController.PlayerAnimatorControl.GunSetEnd();

                _setUpTimeCount = 0;
            }

        }

        public void UpData()
        {
            // �|�[�Y���͉����ł��Ȃ�
            if (GameManager.Instance.PauseManager.PauseCounter > 0) return;


            //�\���Ă���Œ��̏��� 
            //�\���Ă�ԁA�Q�[�W�����炷
            if (_isGunSetUp)
            {
                //���x�̌�������
                _playerController.Move.VelocityDeceleration();

            }       //�\���ĂȂ��Ƃ��̏����B�Q�[�W���񕜂���



            //�󒆂ł͏o���Ȃ�
            if (!_playerController.GroungChecker.IsHit(_playerController.Move.MoveHorizontalDir)) return;

            // �ߐڍU�����A�͏o���Ȃ�
            if (_playerController.Proximity.IsProximityNow) return;

            if (_playerController.PlayerAnimatorControl.IsAnimationNow) return;

            //��𒆂̍\���̓��͎�t
            //if (_playerController.Avoidance.IsAvoidanceNow && )
            //{
            //    return;
            //}

            //�\���̓��͂𗣂����ꍇ
            if (_playerController.InputManager.IsReleased[InputType.GunSetUp] && _isGunSetUp)
            {
                if (_playerController.Avoidance.IsAvoidanceNow)
                {
                    _playerController.Avoidance.EndThereAvoidance();
                }

                //�d�͂�߂�
                _playerController.Rigidbody2D.gravityScale = 1f;

                //���x����
                EndSlowTime();

                //�A�j���[�V������ݒ�
                _playerController.PlayerAnimatorControl.GunSetEnd();

                //�\���ɂ����鎞�Ԃ̌v�������Z�b�g
                _setUpTimeCount = 0;

                //�\���A��Ԃ�����
                _isGunSetUp = false;

                //�ً}�ŉ�������A��False��
                _isEmergencyStopSlowTime = false;

                //������I��
                _playerController.Avoidance.EndThereAvoidance();
            }

            //�\���̃{�^������������
            if (_playerController.InputManager.IsPressed[InputType.GunSetUp] && !_isGunSetUp)
            {
                _isGunSetUp = true;

                //�����[�h�𒆒f����
                _playerController.RevolverOperator.StopRevolverReLoad();

                //�\��
                _playerController.PlayerAnimatorControl.GunSet(false);

                //�r�̊p�x��ݒ�
                _playerController.BodyAnglSetteing.AimingSet();

                //�Ə���`��
                _playerController.Revolver.OnDrawAimingLine();

                DoSlow();
            }
        }


        /// <summary>�e�s���I����</summary>
        /// �\�����{�^����b�������ǂ������m�F����
        public void CheckRelesedSetUp()
        {
            if (!_playerController.InputManager.IsExist[InputType.GunSetUp])
            {
                if (_isGunSetUp)
                {
                    _isGunSetUp = false;

                    _playerController.PlayerAnimatorControl.GunSetEnd();

                    _setUpTimeCount = 0;

                    EndSlowTime();

                    _isEmergencyStopSlowTime = false;
                }

            }     //�\���{�^���𗣂�����\��������
        }

        public void DoSlow()
        {
            /////TEST�p............
            _testSlowTimeText.SetActive(true);

            _isSlowTimeNow = true;

            GameManager.Instance.AudioManager.PlaySE("CueSheet_Gun", "SE_Player_Slow");
            _beatSoundIndex = GameManager.Instance.AudioManager.PlaySE("CueSheet_Gun", "SE_HeartBeat");

            GameManager.Instance.ShaderPropertyController.MonochromeController.SetMonoBlend(1, 0.2f);

            Debug.Log("����x������");
            // ���Ԃ̑��x���������ɂ���B
            GameManager.Instance.TimeController.ChangeTimeSpeed(true);
        }


        public void EndSlowTime()
        {
            if (_isEmergencyStopSlowTime || !_isGunSetUp) return;

            /////TEST�p............
            _testSlowTimeText.SetActive(false);

            _isSlowTimeNow = false;

            //�x�����鎞�̉�
            GameManager.Instance.AudioManager.PlaySE("CueSheet_Gun", "SE_Player_SlowFinish");
            GameManager.Instance.AudioManager.StopSE(_beatSoundIndex);
            _beatSoundIndex = -1;

            GameManager.Instance.ShaderPropertyController.MonochromeController.SetMonoBlend(0, 0.2f);

            Debug.Log("����߂�");
            // ���Ԃ̑��x�����Ƃ̏�Ԃɖ߂��B
            GameManager.Instance.TimeController.ChangeTimeSpeed(false);
        }

        public void StopSlowSE()
        {
            GameManager.Instance.AudioManager.StopSE(_beatSoundIndex);
            _beatSoundIndex = -1;
        }

        public void CanselSetUpping()
        {
            _playerController.PlayerAnimatorControl.GunSetEnd();

            _setUpTimeCount = 0;

            _isEmergencyStopSlowTime = false;

            EndSlowTime();

            _isGunSetUp = false;
        }

        public void EmergencyStopSlowTime()
        {
            //�\���Ă����Ԃ���Ȃ�������Ă΂Ȃ�
            if (!_isGunSetUp) return;

            //���x���łȂ�������Ă΂Ȃ�
            if (!_isSlowTimeNow) return;

            //���ɂ��̊֐���ǂ�ł�����Ă΂Ȃ�
            if (_isEmergencyStopSlowTime) return;



            EndSlowTime();

            _isEmergencyStopSlowTime = true;
        }

    }


}
