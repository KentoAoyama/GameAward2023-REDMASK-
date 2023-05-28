using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;


namespace Player
{

    [System.Serializable]
    public class PlayerAnimationControl
    {
        [Header("�G���E�����Ȃ�True�ɂ��Ă�������")]
        [Tooltip("�G���E�����Ȃ�True��"), SerializeField]
        private bool _isRightDirOnPictuer = true;

        [Header("=====�I�u�W�F�N�g�̐ݒ�======")]
        [Header("�\���̃I�u�W�F�N�g")]
        [SerializeField] private List<GameObject> _nomalGunSetUpObjects = new List<GameObject>();
        [Header("��𒆂̍\���̃I�u�W�F�N�g")]
        [SerializeField] private List<GameObject> _avoidGunSetUpObjects = new List<GameObject>();
        [Header("��𒆂̍\���̍������̃I�u�W�F�N�g")]
        [SerializeField] private GameObject _avoidLeftBody;
        [Header("��𒆂̍\���̉E�����̃I�u�W�F�N�g")]
        [SerializeField] private GameObject _avoidRightBody;
        [Header("�ʏ�̃I�u�W�F�N�g")]
        [SerializeField] private GameObject _animationObject;

        public GameObject AnimObject => _animationObject;

        [Header("=====Animation��======")]
        [Header("�ߐڍU��")]
        [SerializeField] private string _animProximity = "Player_Proximity";
        [Header("���C")]
        [SerializeField] private string _animFire = "Player_Fire";
        [Header("���")]
        [SerializeField] private string _animAvoid = "Player_Avoid";
        [Header("��𒆂ɔ��C")]
        [SerializeField] private string _animAvoidFire = "Player_Fire";
        [Header("�����[�h�͂���")]
        [SerializeField] private string _animReLoadStart = "Player_ReLoadStart";
        [Header("�����[�h�A�e���Ă߂�")]
        [SerializeField] private string _animReLoadNow = "Player_ReLoadNow";
        [Header("�����[�h�A�I���")]
        [SerializeField] private string _animReLoadEnd = "Player_ReLoadEnd";
        [Header("���S")]
        [SerializeField] private string _animDead = "Player_Dead";



        [Header("====Animator�̃p�����[�^��====")]
        [Header("���xX")]
        private string _xVelocityParameta = "SpeedX";
        [Header("���xY")]
        private string _yVelocityParameta = "SpeedY";
        [Header("�ݒu")]
        private string _isGroundParameta = "IsGround";

        [Header("���S�A�j���[�V�����BAnimator�̖��O")]
        [Tooltip("�G���E�����Ȃ�True��"), SerializeField]
        private string _deadAnimName = "���S�A�j���[�V�����BAnimator�̖��O";


        /// <summary>���݂̃L�����̈ړ��̓��͂̌���</summary>
        private float _moveHorizontalDir = 1;

        /// <summary>���C�̃A�j���[�V�����Đ������ǂ���</summary>
        private bool _isFire = false;

        /// <summary>���݃A�j���[�V�����Đ������ǂ���</summary>
        private bool _isAnimationNow = false;


        private PlayerController _playerController;

        public bool IsAnimationNow => _isAnimationNow;
        public float MoveDir { get => _moveHorizontalDir; set => _moveHorizontalDir = value; }
        public bool IsPause { get; private set; } = false;

        /// <summary>�Đ�������A�j���[�V�����̎��</summary>
        public enum AnimaKind
        {
            /// <summary>���C</summary>
            Fire,
            /// <summary>�ߐڍU�� </summary>
            Proximity,
            /// <summary>�W�����v</summary>
            Jump,
            /// <summary>���</summary>
            Avoid,
            /// <summary>��𒆂ɁA���C</summary>
            AvoidFire,
            /// <summary>�����[�h�J�n</summary>
            ReLoadStart,
            /// <summary>�����[�h�A�e�Ă�</summary>
            ReLoad,
            /// <summary>�����[�h�A�I��</summary>
            ReLoadEnd,
            /// <summary>���S</summary>
            Dead,
        }

        public void Init(PlayerController playerController)
        {
            _playerController = playerController;
        }

        public async void Pause()
        {
            await UniTask.WaitUntil(() => _playerController != null);
            // Update���~����
            IsPause = true;

            if (_playerController.PlayerAnim != null)
            {
                _playerController.PlayerAnim.speed = 0;
            }
        }
        public void Resume()
        {
            // Update���ĊJ����
            IsPause = false;

            if (_playerController.PlayerAnim != null)
            {
                _playerController.PlayerAnim.speed = 1;
            }
        }

        /// <summary>�A�j���[�^�[�̃p�����[�^��ݒ�</summary>
        public void SetAnimatorParameters()
        {
            float x = Mathf.Abs(_playerController.Rigidbody2D.velocity.x);
            _playerController.PlayerAnim.SetFloat(_xVelocityParameta, x);

            float y = Mathf.Abs(_playerController.Rigidbody2D.velocity.y);
            _playerController.PlayerAnim.SetFloat(_yVelocityParameta, y);

            _playerController.PlayerAnim.SetBool(_isGroundParameta, _playerController.GroungChecker.IsHit(_playerController.Move.MoveHorizontalDir));
            _playerController.PlayerAnim.SetBool("IsGunSetUp", _playerController.GunSetUp.IsGunSetUp);
        }

        /// <summary>�\����Object��\�����A�A�j���[�V�����p��Object���\���ɂ���</summary>
        /// <param name="isAvoid"></param>
        public void GunSet(bool isAvoid)
        {
            //�A�j���[�V�����Đ����͍\���͏o���Ȃ�
            if (_isAnimationNow) return;

            //�A�j���[�V�����p�̃I�u�W�F�N�g���\���ɂ���
            _animationObject.SetActive(false);


            //��𒆂͉��p�̍\����\��
            if (_playerController.Avoidance.IsAvoidanceNow)
            {
                //���p�̍\���̃I�u�W�F�N�g��\��
                _avoidGunSetUpObjects.ForEach(i => i.SetActive(true));
                //�ʏ�̍\���̃I�u�W�F�N�g���\��
                _nomalGunSetUpObjects.ForEach(i => i.SetActive(false));

                //�v���C���[�̌����ɂ���āA�E������������؂�ւ���
                if (_playerController.Player.transform.localScale.x == 1)
                {
                    _avoidRightBody.SetActive(true);
                    _avoidLeftBody.SetActive(false);
                }
                else
                {
                    _avoidRightBody.SetActive(false);
                    _avoidLeftBody.SetActive(true);
                }
            }    //�ʏ�̍\����\��
            else
            {
                //����̍\���̃I�u�W�F�N�g���\��
                _avoidGunSetUpObjects.ForEach(i => i.SetActive(false));
                //�ʏ�̍\���̃I�u�W�F�N�g��\��
                _nomalGunSetUpObjects.ForEach(i => i.SetActive(true));
            }
        }

        /// <summary>�\�����I��������ɍ\���̃I�u�W�F�N�g���\���ɂ���</summary>
        public void GunSetEnd()
        {
            //�A�j���[�V�����p�̃I�u�W�F�N�g��\��
            _animationObject.SetActive(true);

            //���ꂼ��̍\���̃I�u�W�F�N�g���\��
            if (_playerController.GunSetUp.IsGunSetUp)
            {
                _avoidGunSetUpObjects.ForEach(i => i.SetActive(false));
                _nomalGunSetUpObjects.ForEach(i => i.SetActive(false));
            }
        }

        /// <summary>����̃A�j���[�V�����p�����[�^�[�ݒ�</summary>
        /// <param name="isTrue"></param>
        public void SetBoolAvoid(bool isTrue)
        {
            _playerController.PlayerAnim.SetBool("IsAvoid", isTrue);
        }


        /// <summary>�v���C���[�̉���̃A�j���[�V�������Đ�</summary>
        /// <param name="animationKind">�Đ��������A�j���[�V����</param>
        public void PlayAnimation(PlayerAnimationControl.AnimaKind animationKind)
        {
            //�A�j���[�V�����p�̃I�u�W�F�N�g��\��
            _animationObject.SetActive(true);

            //�\���̃I�u�W�F�N�g���\��
            _nomalGunSetUpObjects.ForEach(i => i.SetActive(false));
            _avoidGunSetUpObjects.ForEach(i => i.SetActive(false));


            if (animationKind == AnimaKind.Fire)
            {
                //���݃A�j���[�V�����Đ���
                _isAnimationNow = true;

                if (_playerController.GunSetUp.IsGunSetUp)
                {
                    _animationObject.transform.localScale = new Vector3(_playerController.BodyAnglSetteing.AnimationScaleX * _playerController.Player.transform.localScale.x, 1, 1);
                }

                _playerController.PlayerAnim.Play(_animFire, 0, 0);
                _isFire = true;
            }
            else if (animationKind == AnimaKind.Proximity)
            {
                //���݃A�j���[�V�����Đ���
                _isAnimationNow = true;

                _playerController.PlayerAnim.Play(_animProximity);
            }
            else if (animationKind == AnimaKind.ReLoadStart)
            {
                _playerController.PlayerAnim.Play(_animReLoadStart);
                _isAnimationNow = false;
            }
            else if (animationKind == AnimaKind.ReLoad)
            {
                _playerController.PlayerAnim.Play(_animReLoadNow, 0, 0);
                _isAnimationNow = false;
            }
            else if (animationKind == AnimaKind.ReLoadEnd)
            {
                if (_isFire) return;

                _playerController.PlayerAnim.Play(_animReLoadEnd);
                _isAnimationNow = false;
            }
            else if (animationKind == AnimaKind.Avoid)
            {
                //���݃A�j���[�V�����Đ���
                _isAnimationNow = true;

                if (_playerController.GunSetUp.IsGunSetUp)
                {
                    _animationObject.transform.localScale = new Vector3(_playerController.BodyAnglSetteing.AnimationScaleX * _playerController.Player.transform.localScale.x, 1, 1);
                }
                _playerController.PlayerAnim.Play(_animAvoid, 0, 0);
                _playerController.PlayerAnim.SetBool("IsAvoid", true);
            }
            else if (animationKind == AnimaKind.AvoidFire)
            {
                //���݃A�j���[�V�����Đ���
                _isAnimationNow = true;

                if (_playerController.GunSetUp.IsGunSetUp)
                {
                    _animationObject.transform.localScale = new Vector3(_playerController.BodyAnglSetteing.AnimationScaleX * _playerController.Player.transform.localScale.x, 1, 1);
                }

                _playerController.PlayerAnim.Play(_animAvoidFire);
                _isFire = true;
            }
            else if (animationKind == AnimaKind.Dead)
            {
                //���݃A�j���[�V�����Đ���
                _isAnimationNow = true;

                _playerController.PlayerAnim.Play(_animDead);
            }
        }

        /// <summary>�A�j���[�V�������I��������Ƃ�ʒm</summary>
        public void EndAnimation()
        {
            //���݃A�j���[�V�����Đ����ł͂Ȃ�
            _isAnimationNow = false;

            //���C���ł͂Ȃ�
            _isFire = false;

            //�A�j���[�V�����I��莞�ɍ\���Ă邩�ǂ������m�F
            _playerController.GunSetUp.AnimEndSetUpCheck();

            //�A�j���[�V�����p�̃I�u�W�F�N�g�����ɖ߂�
            _animationObject.transform.localScale = new Vector3(1, 1, 1);
        }



        /// <summary>�v���C���[�̊G�̕������A�ς���
        ///�N���X:Move ����Ă�ł���B</summary>
        /// <param name="dir">�ړ��œ��͂�������</param>
        public void SetPlayerDir(float dir)
        {
            if (dir == 0) return;

            _moveHorizontalDir = dir;

            //�v���C���[�̃C���X�g�̌����ɂ���āA���E���]�̎d����ς���
            if (_isRightDirOnPictuer)
            {
                _playerController.Player.transform.localScale = new Vector3(_moveHorizontalDir * Mathf.Abs(_playerController.Player.transform.localScale.x), _playerController.Player.transform.localScale.y, _playerController.Player.transform.localScale.z);
            }
            else
            {
                _playerController.Player.transform.localScale = new Vector3(-_moveHorizontalDir * Mathf.Abs(_playerController.Player.transform.localScale.x), _playerController.Player.transform.localScale.y, _playerController.Player.transform.localScale.z);
            }
        }

        /// <summary>���S�A�j���[�V�������Đ�</summary>
        public void Dead()
        {
            _playerController.PlayerAnim.Play(_deadAnimName);
        }
    }
}