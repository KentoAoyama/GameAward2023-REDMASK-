using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class PlayerAnimEnd : MonoBehaviour
    {
        [SerializeField] private PlayerController _playerController;


        /// <summary>�A�j���[�V�����Đ����I���������Ƃ�ʒB</summary>
        public void AnimEnd()
        {
            ////���Ƃ��ƍ\����Ԃł������Ȃ�A�A�j���[�V�����p�̃I�u�W�F�N�g�͔�\���ɂ���
            //if (_playerController.GunSetUp.IsGunSetUp)
            //{
            //    gameObject.SetActive(false);
            //}

            //�A�j���[�V�����Đ����I�����
            _playerController.PlayerAnimatorControl.EndAnimation();
        }

        /// <summary>���C���I���������ʒB</summary>
        public void FireEnd()
        {
            _playerController.RevolverOperator.IsFireNow = false;
        }

        public void EndProirity()
        {
            _playerController.Proximity.AttackEnd();
        }

    }
}
