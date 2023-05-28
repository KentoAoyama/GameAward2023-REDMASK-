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

        [Header("=====オブジェクトの設定======")]
        [Header("構えのオブジェクト")]
        [SerializeField] private List<GameObject> _nomalGunSetUpObjects = new List<GameObject>();
        [Header("回避中の構えのオブジェクト")]
        [SerializeField] private List<GameObject> _avoidGunSetUpObjects = new List<GameObject>();
        [Header("回避中の構えの左向きのオブジェクト")]
        [SerializeField] private GameObject _avoidLeftBody;
        [Header("回避中の構えの右向きのオブジェクト")]
        [SerializeField] private GameObject _avoidRightBody;
        [Header("通常のオブジェクト")]
        [SerializeField] private GameObject _animationObject;

        public GameObject AnimObject => _animationObject;

        [Header("=====Animation名======")]
        [Header("近接攻撃")]
        [SerializeField] private string _animProximity = "Player_Proximity";
        [Header("発砲")]
        [SerializeField] private string _animFire = "Player_Fire";
        [Header("回避")]
        [SerializeField] private string _animAvoid = "Player_Avoid";
        [Header("回避中に発砲")]
        [SerializeField] private string _animAvoidFire = "Player_Fire";
        [Header("リロードはじめ")]
        [SerializeField] private string _animReLoadStart = "Player_ReLoadStart";
        [Header("リロード、弾を籠める")]
        [SerializeField] private string _animReLoadNow = "Player_ReLoadNow";
        [Header("リロード、終わり")]
        [SerializeField] private string _animReLoadEnd = "Player_ReLoadEnd";
        [Header("死亡")]
        [SerializeField] private string _animDead = "Player_Dead";



        [Header("====Animatorのパラメータ名====")]
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

        /// <summary>発砲のアニメーション再生中かどうか</summary>
        private bool _isFire = false;

        /// <summary>現在アニメーション再生中かどうか</summary>
        private bool _isAnimationNow = false;


        private PlayerController _playerController;

        public bool IsAnimationNow => _isAnimationNow;
        public float MoveDir { get => _moveHorizontalDir; set => _moveHorizontalDir = value; }
        public bool IsPause { get; private set; } = false;

        /// <summary>再生させるアニメーションの種類</summary>
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
            /// <summary>回避中に、発砲</summary>
            AvoidFire,
            /// <summary>リロード開始</summary>
            ReLoadStart,
            /// <summary>リロード、弾籠め</summary>
            ReLoad,
            /// <summary>リロード、終了</summary>
            ReLoadEnd,
            /// <summary>死亡</summary>
            Dead,
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

        /// <summary>アニメーターのパラメータを設定</summary>
        public void SetAnimatorParameters()
        {
            float x = Mathf.Abs(_playerController.Rigidbody2D.velocity.x);
            _playerController.PlayerAnim.SetFloat(_xVelocityParameta, x);

            float y = Mathf.Abs(_playerController.Rigidbody2D.velocity.y);
            _playerController.PlayerAnim.SetFloat(_yVelocityParameta, y);

            _playerController.PlayerAnim.SetBool(_isGroundParameta, _playerController.GroungChecker.IsHit(_playerController.Move.MoveHorizontalDir));
            _playerController.PlayerAnim.SetBool("IsGunSetUp", _playerController.GunSetUp.IsGunSetUp);
        }

        /// <summary>構えのObjectを表示し、アニメーション用のObjectを非表示にする</summary>
        /// <param name="isAvoid"></param>
        public void GunSet(bool isAvoid)
        {
            //アニメーション再生中は構えは出来ない
            if (_isAnimationNow) return;

            //アニメーション用のオブジェクトを非表示にする
            _animationObject.SetActive(false);


            //回避中は回避用の構えを表示
            if (_playerController.Avoidance.IsAvoidanceNow)
            {
                //回避用の構えのオブジェクトを表示
                _avoidGunSetUpObjects.ForEach(i => i.SetActive(true));
                //通常の構えのオブジェクトを非表示
                _nomalGunSetUpObjects.ForEach(i => i.SetActive(false));

                //プレイヤーの向きによって、右向き左向きを切り替える
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
            }    //通常の構えを表示
            else
            {
                //回避の構えのオブジェクトを非表示
                _avoidGunSetUpObjects.ForEach(i => i.SetActive(false));
                //通常の構えのオブジェクトを表示
                _nomalGunSetUpObjects.ForEach(i => i.SetActive(true));
            }
        }

        /// <summary>構えが終わった時に構えのオブジェクトを非表示にする</summary>
        public void GunSetEnd()
        {
            //アニメーション用のオブジェクトを表示
            _animationObject.SetActive(true);

            //それぞれの構えのオブジェクトを非表示
            if (_playerController.GunSetUp.IsGunSetUp)
            {
                _avoidGunSetUpObjects.ForEach(i => i.SetActive(false));
                _nomalGunSetUpObjects.ForEach(i => i.SetActive(false));
            }
        }

        /// <summary>回避のアニメーションパラメーター設定</summary>
        /// <param name="isTrue"></param>
        public void SetBoolAvoid(bool isTrue)
        {
            _playerController.PlayerAnim.SetBool("IsAvoid", isTrue);
        }


        /// <summary>プレイヤーの回避のアニメーションを再生</summary>
        /// <param name="animationKind">再生したいアニメーション</param>
        public void PlayAnimation(PlayerAnimationControl.AnimaKind animationKind)
        {
            //アニメーション用のオブジェクトを表示
            _animationObject.SetActive(true);

            //構えのオブジェクトを非表示
            _nomalGunSetUpObjects.ForEach(i => i.SetActive(false));
            _avoidGunSetUpObjects.ForEach(i => i.SetActive(false));


            if (animationKind == AnimaKind.Fire)
            {
                //現在アニメーション再生中
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
                //現在アニメーション再生中
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
                //現在アニメーション再生中
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
                //現在アニメーション再生中
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
                //現在アニメーション再生中
                _isAnimationNow = true;

                _playerController.PlayerAnim.Play(_animDead);
            }
        }

        /// <summary>アニメーションが終わったことを通知</summary>
        public void EndAnimation()
        {
            //現在アニメーション再生中ではない
            _isAnimationNow = false;

            //発砲中ではない
            _isFire = false;

            //アニメーション終わり時に構えてるかどうかを確認
            _playerController.GunSetUp.AnimEndSetUpCheck();

            //アニメーション用のオブジェクトを元に戻す
            _animationObject.transform.localScale = new Vector3(1, 1, 1);
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

        /// <summary>死亡アニメーションを再生</summary>
        public void Dead()
        {
            _playerController.PlayerAnim.Play(_deadAnimName);
        }
    }
}