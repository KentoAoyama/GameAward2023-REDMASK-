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
        [Header("絵が右向きならTrueにしてください")]
        [Tooltip("絵が右向きならTrueに"), SerializeField]
        private bool _isRightDirOnPictuer = true;



        [Header("構えのオブジェクト")]
        [SerializeField] private List<GameObject> _nomalAnim = new List<GameObject>();

        [Header("回避中の構えのオブジェクト")]
        [SerializeField] private List<GameObject> _avoidAnim = new List<GameObject>();

        [Header("通常のオブジェクト")]
        [SerializeField] private GameObject _fireAnim;

        [Header("Animation名")]



        [Header("ジャンプ")]
        [SerializeField] private string _animJump = "Player_JumpStart";
        [Header("発砲")]
        [SerializeField] private string _animFire = "Player_Fire";
        [Header("リロードはじめ")]
        [SerializeField] private string _animReLoadStart = "Player_ReLoadStart";
        [Header("リロード、弾を籠める")]
        [SerializeField] private string _animReLoadNow = "Player_ReLoadNow";
        [Header("リロード、終わり")]
        [SerializeField] private string _animReLoadEnd = "Player_ReLoadEnd";
        [Header("近接攻撃")]
        [SerializeField] private string _animProximity = "Player_Proximity";
        [Header("回避")]
        [SerializeField] private string _animAvoid = "Player_Avoid";

        [Header("Animatorのパラメータ名")]
        [Header("速度X")]
        private string _xVelocityParameta = "SpeedX";

        [Header("速度Y")]
        private string _yVelocityParameta = "SpeedY";
        [Header("設置")]
        private string _isGroundParameta = "IsGround";

        [Header("死亡アニメーション。Animatorの名前")]
        [Tooltip("絵が右向きならTrueに"), SerializeField]
        private string _deadAnimName = "死亡アニメーション。Animatorの名前";


        /// <summary>現在のキャラの移動の入力の向き</summary>
        private float _moveHorizontalDir = 1;

        private bool _isFire = false;

        private bool _isAnimationNow = false;
        private PlayerController _playerController;

        public bool IsAnimationNow => _isAnimationNow;
        public float MoveDir { get => _moveHorizontalDir; set => _moveHorizontalDir = value; }
        public bool IsPause { get; private set; } = false;

        public enum AnimaKind
        {
            /// <summary>発砲</summary>
            Fire,
            /// <summary>近接攻撃 </summary>
            Proximity,
            /// <summary>ジャンプ</summary>
            Jump,
            /// <summary>回避</summary>
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

        /// <summary>死亡アニメーションを再生</summary>
        public void Dead()
        {
            _playerController.PlayerAnim.Play(_deadAnimName);
        }

    }
}