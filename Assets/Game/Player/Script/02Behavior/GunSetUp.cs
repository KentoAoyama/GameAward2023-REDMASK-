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

        public bool IsSlowTimeNow => _isSlowTimeNow;

        private PlayerController _playerController = null;

        private bool _isEmergencyStopSlowTime = false;

        private bool _isCanselSutUping = false;

        /// <summary>�\���Ă��邩�ǂ���</summary>
        private bool _isGunSetUp = false;

        private bool _isGunSetUping = false;

        private bool _isNoGage = false;

        private bool _isReserveSetUpAvoid = false;

        public bool IsGunSetUp => _isGunSetUp;

        public bool IsGunSetUpping => _isGunSetUping;


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

            CheckRelesedSetUp();


        }

        public void Init(PlayerController playerController)
        {
            _playerController = playerController;
        }


        public void UpData()
        {
            if (GameManager.Instance.PauseManager.PauseCounter > 0)
            {
                return;
            } // �|�[�Y���͉����ł��Ȃ�

            if (!_playerController.GroungChecker.IsHit(_playerController.Move.MoveHorizontalDir))
            {
                return;
            }   //�󒆂ł͏o���Ȃ�


            // �ߐڍU�����A
            if (_playerController.Proximity.IsProximityNow) return;

            if (_playerController.Avoidance.IsAvoidanceNow && !_playerController.GunSetUp.IsGunSetUp && _playerController.InputManager.IsPressed[InputType.GunSetUp] && !_isReserveSetUpAvoid)
            {
                _isReserveSetUpAvoid = true;
            }

            if (_isReserveSetUpAvoid)
            {
                if (!_playerController.PlayerAnimatorControl.IsAnimationNow)
                {
                    _isReserveSetUpAvoid = false;

                    //�d�͂�߂�
                    _playerController.Rigidbody2D.gravityScale = 1f;

                    _isGunSetUp = true;
                    _isGunSetUping = false;

                    if (!_isSlowTimeNow)
                    {
                        DoSlow();
                    }

                    _playerController.PlayerAnimatorControl.GunSet(true);
                }
            }



            //�\���̓��͂𗣂����ꍇ
            if (_playerController.InputManager.IsReleased[InputType.GunSetUp])
            {
                EndSlowTime();

                //�A�j���[�V������ݒ�
                _playerController.PlayerAnimatorControl.GunSetEnd();

                //�\���ɂ����鎞�Ԃ̌v�������Z�b�g
                _setUpTimeCount = 0;

                //�\���A��Ԃ�����
                _isGunSetUp = false;

                //�\�����A�̏�Ԃ�����
                _isGunSetUping = false;

                //�ً}�ŉ�������A��False��
                _isEmergencyStopSlowTime = false;

                //�\�����A��bool��false��
                _isCanselSutUping = false;


                _playerController.Avoidance.EndThereAvoidance();
            }

            //�\���̃{�^������������
            if (_playerController.InputManager.IsPressed[InputType.GunSetUp] )
            {
                //�\���Ă�Œ�
                _isGunSetUping = true;
            }


            //�\���Ă͂��߂Ă���Ԃ̏����B��莞�ԍ\���{�^���������Ă�����\����
            if (_isGunSetUping)
            {
                if (_isGunSetUp || _isCanselSutUping)
                {
                    return;
                }

                _setUpTimeCount += Time.deltaTime;

                if (_setUpTimeCount >= _setUpTime)
                {
                    //�d�͂�߂�
                    _playerController.Rigidbody2D.gravityScale = 1f;

                    _isGunSetUp = true;
                    _isGunSetUping = false;

                    DoSlow();
                    _playerController.PlayerAnimatorControl.GunSet(false);

                    _isNoGage = false;
                }

                //���x�̌�������
                _playerController.Move.VelocityDeceleration();

            }   //�\���{�^������������\����


            //�\���Ă���Œ��̏��� 
            //�\���Ă�ԁA�Q�[�W�����炷
            if (_isGunSetUp)
            {
                //���x�̌�������
                _playerController.Move.VelocityDeceleration();

            }       //�\���ĂȂ��Ƃ��̏����B�Q�[�W���񕜂���
        }


        public void CheckRelesedSetUp()
        {
            if (!_playerController.InputManager.IsExist[InputType.GunSetUp])
            {
                if (_isGunSetUp)
                {
                    _playerController.PlayerAnimatorControl.GunSetEnd();

                    _isGunSetUp = false;

                    _isGunSetUping = false;

                    _setUpTimeCount = 0;

                    EndSlowTime();

                    _isEmergencyStopSlowTime = false;
                    _isCanselSutUping = false;
                }

            }     //�\���{�^���𗣂�����\��������
        }

        public void DoSlow()
        {
            /////TEST�p............
            _testSlowTimeText.SetActive(true);

            _isSlowTimeNow = true;

            GameManager.Instance.AudioManager.PlaySE("CueSheet_Gun", "SE_Player_Slow");

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

            _isNoGage = true;

            //�x�����鎞�̉�
            GameManager.Instance.AudioManager.PlaySE("CueSheet_Gun", "SE_Player_SlowFinish");

            GameManager.Instance.ShaderPropertyController.MonochromeController.SetMonoBlend(0, 0.2f);

            Debug.Log("����߂�");
            // ���Ԃ̑��x�����Ƃ̏�Ԃɖ߂��B
            GameManager.Instance.TimeController.ChangeTimeSpeed(false);
        }

        public void CanselSetUpping()
        {
            if (_isGunSetUp || _isCanselSutUping) return;

            _isGunSetUping = false;
            _isCanselSutUping = true;
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
