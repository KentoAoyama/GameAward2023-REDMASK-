using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class PlayerAnimEnd : MonoBehaviour
    {
        [SerializeField] private PlayerController _playerController;


        public void AnimEnd()
        {
            if (_playerController.GunSetUp.IsGunSetUp)
            {
                gameObject.SetActive(false);
            }

            _playerController.PlayerAnimatorControl.EndAnimation();
        }

    }
}
