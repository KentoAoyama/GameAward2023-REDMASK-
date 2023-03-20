using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Player
{
    /// <summary>近接攻撃</summary>
    public class Proximity
    {

        private PlayerController _playerController = null;

        public void Init(PlayerController playerController)
        {
            _playerController = playerController;
        }

        void Start()
        {

        }


        void Update()
        {
            if (GameManager.Instance.PauseManager.PauseCounter > 0)
            {
                return;
            } // ポーズ中は何もできない

            if (_playerController.Avoidance.IsAvoiddanceNow)
            {
                return;
            } //回避中はできない


        }
    }
}