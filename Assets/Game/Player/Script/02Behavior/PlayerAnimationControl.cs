using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace Player
{

    [System.Serializable]
    public class PlayerAnimationControl
    {
        [Header("�G���E�����Ȃ�True�ɂ��Ă�������")]
        [Tooltip("�G���E�����Ȃ�True��"), SerializeField]
        private bool _isRightDirOnPictuer = true;

        [Header("Animator�̃p�����[�^��")]

        [Header("����_float")]
        private string _xVelocityParameta = "";


        [Header("�ݒu����_bool")]
        private bool _isGroundParameta = false;

        [Header("���S�A�j���[�V�����BAnimator�̖��O")]
        [Tooltip("�G���E�����Ȃ�True��"), SerializeField]
        private string _deadAnimName = "���S�A�j���[�V�����BAnimator�̖��O";


        /// <summary>���݂̃L�����̈ړ��̓��͂̌���</summary>
        private float _moveHorizontalDir = 1;







        public float MoveDir { get => _moveHorizontalDir; set => _moveHorizontalDir = value; }


        public bool IsPause { get; private set; } = false;

        private PlayerController _playerController;


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
                _playerController.Player.transform.localScale = new Vector3(_moveHorizontalDir, 1, 1);
            }
            else
            {
                _playerController.Player.transform.localScale = new Vector3(-_moveHorizontalDir, 1, 1);
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