using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class PlayerAnimEnd : MonoBehaviour
    {
        [SerializeField] private PlayerController _playerController;


        /// <summary>アニメーション再生が終了したことを通達</summary>
        public void AnimEnd()
        {
            ////もともと構え状態であったなら、アニメーション用のオブジェクトは非表示にする
            //if (_playerController.GunSetUp.IsGunSetUp)
            //{
            //    gameObject.SetActive(false);
            //}

            //アニメーション再生が終わった
            _playerController.PlayerAnimatorControl.EndAnimation();
        }

        /// <summary>発砲が終わった事を通達</summary>
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
