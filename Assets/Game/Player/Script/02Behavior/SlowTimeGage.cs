using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Player
{
    [System.Serializable]
    public class SlowTimeGage 
    {
        private PlayerController _playerController = null;

        public void Init(PlayerController playerController)
        {
            _playerController = playerController;
        }




    }
}