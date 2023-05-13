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



        [Header("�\���̃I�u�W�F�N�g")]
        [SerializeField] private List<GameObject> _nomalAnim = new List<GameObject>();

        [Header("��𒆂̍\���̃I�u�W�F�N�g")]
        [SerializeField] private List<GameObject> _avoidAnim = new List<GameObject>();

        [Header("�ʏ�̃I�u�W�F�N�g")]
        [SerializeField] private GameObject _fireAnim;

        [Header("Animation��")]



        [Header("�W�����v")]
        [SerializeField] private string _animJump = "Player_JumpStart";
        [Header("���C")]
        [SerializeField] private string _animFire = "Player_Fire";
        [Header("�����[�h�͂���")]
        [SerializeField] private string _animReLoadStart = "Player_ReLoadStart";
        [Header("�����[�h�A�e���Ă߂�")]
        [SerializeField] private string _animReLoadNow = "Player_ReLoadNow";
        [Header("�����[�h�A�I���")]
        [SerializeField] private string _animReLoadEnd = "Player_ReLoadEnd";
        [Header("�ߐڍU��")]
        [SerializeField] private string _animProximity = "Player_Proximity";
        [Header("���")]
        [SerializeField] private string _animAvoid = "Player_Avoid";

        [Header("Animator�̃p�����[�^��")]
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

        private bool _isFire = false;

        private bool _isAnimationNow = false;
        private PlayerController _playerController;

        public bool IsAnimationNow => _isAnimationNow;
        public float MoveDir { get => _moveHorizontalDir; set => _moveHorizontalDir = value; }
        public bool IsPause { get; private set; } = false;

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

            ReLoadStart,
            ReLoad,
            ReLoadEnd,
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


        public void SetAnimatorParameters()
        {
            float x = Mathf.Abs(_playerController.Rigidbody2D.velocity.x);
            _playerController.PlayerAnim.SetFloat(_xVelocityParameta, x);

            float y = Mathf.Abs(_playerController.Rigidbody2D.velocity.y);
            _playerController.PlayerAnim.SetFloat(_yVelocityParameta, y);

            _playerController.PlayerAnim.SetBool(_isGroundParameta, _playerController.GroungChecker.IsHit(_playerController.Move.MoveHorizontalDir));
        }

        public void GunSet(bool isAvoid)
        {
            _fireAnim.SetActive(false);
            Debug.Log(_playerController.Avoidance.IsAvoidanceNow);

            if (_playerController.Avoidance.IsAvoidanceNow)
            {

                _avoidAnim.ForEach(i => i.SetActive(true));
                _nomalAnim.ForEach(i => i.SetActive(false));
            }
            else
            {

                if (_playerController.GunSetUp.IsGunSetUp)
                {
                    _avoidAnim.ForEach(i => i.SetActive(false));
                    _nomalAnim.ForEach(i => i.SetActive(true));
                }
            }


        }

        public void GunSetEnd()
        {

            _fireAnim.SetActive(true);
            if (_playerController.GunSetUp.IsGunSetUp)
            {
                _avoidAnim.ForEach(i => i.SetActive(false));
                _nomalAnim.ForEach(i => i.SetActive(false));
            }
        }

        public void SetBoolAvoid(bool isTrue)
        {
            _playerController.PlayerAnim.SetBool("IsAvoid", isTrue);
        }


        public void PlayAnimation(PlayerAnimationControl.AnimaKind animationKind)
        {
            _isAnimationNow = true;
            _fireAnim.SetActive(true);


            _nomalAnim.ForEach(i => i.SetActive(false));

            if (animationKind == AnimaKind.Fire)
            {
                _playerController.PlayerAnim.Play(_animFire);
                _isFire = true;
            }
            else if (animationKind == AnimaKind.Proximity)
            {
                _playerController.PlayerAnim.Play(_animProximity);
            }
            else if (animationKind == AnimaKind.Jump)
            {
                _playerController.PlayerAnim.Play(_animJump);
                _isAnimationNow = false;
            }
            else if (animationKind == AnimaKind.ReLoadStart)
            {
                _playerController.PlayerAnim.Play(_animReLoadStart);
                _isAnimationNow = false;
            }
            else if (animationKind == AnimaKind.ReLoad)
            {
                _playerController.PlayerAnim.Play(_animReLoadNow);
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
                _playerController.PlayerAnim.Play(_animAvoid);
                _playerController.PlayerAnim.SetBool("IsAvoid", true);
                _isFire = true;
            }
        }

        public void EndAnimation()
        {
            if (_playerController.GunSetUp.IsGunSetUp)
            {
                if (_playerController.Avoidance.IsAvoidanceNow)
                {
                    _avoidAnim.ForEach(i => i.SetActive(true));
                    _nomalAnim.ForEach(i => i.SetActive(false));
                }
                else
                {
                    _avoidAnim.ForEach(i => i.SetActive(false));
                    _nomalAnim.ForEach(i => i.SetActive(true));
                }

            }
            _isAnimationNow = false;
            _isFire = false;
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



        public void PlayAnim()
        {

        }

        /// <summary>���S�A�j���[�V�������Đ�</summary>
        public void Dead()
        {
            _playerController.PlayerAnim.Play(_deadAnimName);
        }

    }
}