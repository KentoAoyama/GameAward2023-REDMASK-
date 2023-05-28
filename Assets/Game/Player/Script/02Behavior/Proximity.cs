using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Player
{
    /// <summary>近接攻撃</summary>
    /// 
    [System.Serializable]
    public class Proximity
    {
        [Header("近接攻撃のクールタイム")]
        [Tooltip("近接攻撃のクールタイム"), SerializeField]
        private float _attackCoolTime = 4f;
        private float _attackCoolTimeCount = 0;
        /// <summary>攻撃可能かどうか</summary>
        private bool _isCanAttack = true;
        /// <summary>攻撃実行中かどうか</summary>
        private bool _isAttackNow = false;
        public bool IsProximityNow => _isAttackNow;

        private PlayerController _playerController = null;

        public void Init(PlayerController playerController)
        {
            _playerController = playerController;
        }

        void Start()
        {

        }


        public void Update()
        {
            if (GameManager.Instance.PauseManager.PauseCounter > 0)
            {
                return;
            } // ポーズ中は何もできない

            //クールタイムの計測
            CountCoolTime();

            if (_playerController.Avoidance.IsAvoidanceNow || _playerController.RevolverOperator.IsFireNow)
            {
                return;
            } //回避中はできない

            if (_playerController.InputManager.IsPressed[InputType.Proximity])
            {
                //現在攻撃中でない、攻撃可能である、地面についている
                if (!_isAttackNow && _isCanAttack
                    && _playerController.GroungChecker.IsHit(_playerController.DirectionControler.MovementDirectionX))
                {
                    //攻撃中
                    _isAttackNow = true;

                    //リロードを中断する
                    _playerController.RevolverOperator.StopRevolverReLoad();

                    AttckStart();
                }
            }

            if (_isAttackNow)
            {
                _playerController.Move.VelocityDeceleration();
            }
        }


        private void AttckStart()
        {
            //重力を戻す
            _playerController.Rigidbody2D.gravityScale = 1f;

            //アニメーションの再生
            _playerController.PlayerAnimatorControl.PlayAnimation(PlayerAnimationControl.AnimaKind.Proximity);

            //時遅を強制解除
            _playerController.GunSetUp.EmergencyStopSlowTime();

            //攻撃の音
            GameManager.Instance.AudioManager.PlaySE("CueSheet_Gun", "SE_Player_Attack_Knife");
        }

        /// <summary>攻撃が当たったかどうかの判定をとる。アニメーションから呼ぶ</summary>
        public void AttackHitChck()
        {
            var targets = _playerController.ProximityHitChecker.GetCollider(_playerController.Move.MoveHorizontalDir);
            //Debug.Log(targets.Length);

            if (targets.Length > 0)
            {
                //Debug.Log("攻撃対象あり");
                //Hitしたコライダーに対して、ダメージを与えていく
                foreach (var target in targets)
                {
                    // ダメージを加える
                    if (target.TryGetComponent(out IDamageable hit))
                    {
                        //Debug.Log("攻撃実行可能");
                        hit.Damage();
                        return;
                    }
                }
            }
        }

        public void AttackEnd()
        {
            Debug.Log("End");

            //攻撃中
            _isAttackNow = false;

            //攻撃を不可
            _isCanAttack = false;

            _playerController.Move.EndOtherAction();
        }

        /// <summary>クールタイムを計測</summary>
        private void CountCoolTime()
        {
            if (!_isCanAttack)
            {
                _attackCoolTimeCount += Time.deltaTime;

                if (_attackCoolTimeCount >= _attackCoolTime)
                {
                    _isCanAttack = true;
                    _attackCoolTimeCount = 0;
                }
            }
        }
    }
}