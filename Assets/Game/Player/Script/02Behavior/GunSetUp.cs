using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    [System.Serializable]
    public class GunSetUp
    {
        [Header("�\����܂ł̎���(�b)")]
        [SerializeField] private float _setUpTime = 0.2f;

        [Header("�Q�[�W�̍ő�l(�b)")]
        [SerializeField] private float _gageMax = 5f;

        [Header("�Q�[�W�񕜂̑����B1�b*���̒l����")]
        [SerializeField] private float _gageHealPercent = 1f;

        [Header("�Q�[�W���񕜂���܂ł̋󂫎���")]
        [SerializeField] private float _gageHealWaitTime = 1f;

        [Header("�Q�[�W�p�̃X���C�_�[")]
        [SerializeField] private Slider _gageSlider;

        [Header("Test�p�A���x����Text")]
        [SerializeField]
        private GameObject _testSlowTimeText;

        /// <summary>���݂̃Q�[�W��</summary>
        private float _nowGage = 0;

        private float _gageHealWaitTimeCount = 0;

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
        public bool IsGunSetUp => _isGunSetUp;

        public bool IsGunSetUpping => _isGunSetUping;

        public void Init(PlayerController playerController)
        {
            _playerController = playerController;

            _nowGage = _gageMax;

            _gageSlider.maxValue = _gageMax;
        }


        public void UpData()
        {

            if (_playerController.InputManager.IsReleased[InputType.GunSetUp])
            {
                _isGunSetUp = false;

                _isGunSetUping = false;

                _setUpTimeCount = 0;

                _gageHealWaitTimeCount = 0;

                if (_nowGage > 0)
                {
                    EndSlowTime();
                }

                _isEmergencyStopSlowTime = false;
                _isCanselSutUping = false;

            }     //�\���{�^���𗣂�����\��������


            if (_playerController.InputManager.IsPressed[InputType.GunSetUp])
            {
                _isGunSetUping = true;
            }


            if (_isGunSetUping)
            {
                if (_isGunSetUp || _isCanselSutUping)
                {
                    return;
                }

                _setUpTimeCount += Time.deltaTime;

                if (_setUpTimeCount >= _setUpTime)
                {
                    _isGunSetUp = true;
                    _isGunSetUping = false;

                    if (_nowGage > 0)
                    {
                        DoSlow();
                        _isNoGage = false;
                    }
                }

            }   //�\���{�^������������\����



            if (_isGunSetUp)
            {
                if (!_isNoGage)
                {
                    _nowGage -= Time.deltaTime;

                    _gageSlider.value = _nowGage;

                    if (_nowGage <= 0)
                    {
                        _nowGage = 0;

                        EndSlowTime();
                    }
                }

            }   //�\���Ă�ԁA�Q�[�W�����炷
            else
            {
                if (_gageHealWaitTimeCount < _gageHealWaitTime)
                {
                    _gageHealWaitTimeCount += Time.deltaTime;

                }   //�Q�[�W���񕜂���܂ł̎��Ԃ��v��
                else
                {
                    if (_nowGage < _gageMax)
                    {
                        //�Q�[�W����
                        _nowGage += Time.deltaTime * _gageHealPercent;

                        _gageSlider.value = _nowGage;


                        if (Mathf.Abs(_gageMax - _nowGage) < 0.3f)
                        {
                            _nowGage = _gageMax;
                        }
                    }
                }
            }
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
            if (_isEmergencyStopSlowTime) return;

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