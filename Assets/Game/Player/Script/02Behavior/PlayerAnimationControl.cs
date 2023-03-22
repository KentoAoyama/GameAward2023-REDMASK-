using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace Player
{

    [System.Serializable]
    public class PlayerAnimationControl
    {

        [Header("絵が右向きならTrueにしてください")]
        [Tooltip("絵が右向きならTrueに"), SerializeField]
        private bool _isRightDirOnPictuer = true;

        [Tooltip("現在のキャラの絵の向き")]
        private float _isNowPictureDir = 1;

        [Tooltip("現在のキャラの移動の入力の向き")]
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
            // Updateを停止する
            IsPause = true;

            if (_playerController.PlayerAnim != null)
            {
                _playerController.PlayerAnim.speed = 0;
            }
        }
        public void Resume()
        {
            // Updateを再開する
            IsPause = false;

            if (_playerController.PlayerAnim != null)
            {
                _playerController.PlayerAnim.speed = 1;
            }
        }


        /// <summary>プレイヤーの絵の方向を、変える
        ///クラス:Move から呼んでいる。</summary>
        /// <param name="dir">移動で入力した方向</param>
        public void SetPlayerDir(float dir)
        {
            if (dir == 0) return;

            _moveHorizontalDir = dir;

            //プレイヤーのイラストの向きによって、左右反転の仕方を変える
            if (_isRightDirOnPictuer)
            {
                _playerController.Player.transform.localScale= new Vector3(_moveHorizontalDir, 1, 1);
            }
            else
            {
                _playerController.Player.transform.localScale = new Vector3(-_moveHorizontalDir, 1, 1);
            }
        }



        public void PlayAnim()
        {

        }


    }
}