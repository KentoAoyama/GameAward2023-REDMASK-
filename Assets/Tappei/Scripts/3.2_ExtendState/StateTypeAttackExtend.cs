using UnityEngine;

/// <summary>
/// 盾持ち用
/// 一定間隔で攻撃をする状態のクラス
/// </summary>
public class StateTypeAttackExtend : StateTypeAttack
{
    /// <summary>
    /// 間合いを詰めてくる時間を計算するために攻撃範囲に定数をかける
    /// </summary>
    private static readonly float MovingDistanceMag = 15.0f;
    /// <summary>
    /// 次の攻撃までの待ち時間は攻撃モーションを考慮してマイナスの値を設定する
    /// </summary>
    private static readonly float NextAttackDelay = -80.0f;

    private ShieldEnemyController _shieldController;

    /// <summary>
    /// 継承元であるStateTypeAttackクラスとは攻撃の挙動が違うので別の変数を宣言する
    /// こちらは状態の遷移で初期化される
    /// </summary>
    private float _time;
    /// <summary>
    /// 間合いを詰めてくる動作中かどうかのフラグ
    /// </summary>
    private bool _isApproaching;

    public StateTypeAttackExtend(EnemyController controller, StateType stateType)
    : base(controller, stateType) 
    {
        _shieldController = controller as ShieldEnemyController;
        // 最初の1回しか初期化しない(基底クラスと同じ)に変更
        // ↓ここ以外はリファクタリング後から弄っていない
        _time = Controller.Params.AttackRate;
    }

    protected override void Enter()
    {
        // TODO:攻撃の処理が複雑なのでまだ汚い
        _shieldController.PlayAnimation(AnimationName.Idle);
    }

    protected override void Stay()
    {
        if (TransitionDefeated()) return;
        if (TransitionReflection()) return;
        AttackAtInterval();
        if (Transition()) return;
    }

    protected override void Exit()
    {
        _time = 0;
        _isApproaching = false;
    }

    /// <summary>
    /// 時間経過で前方に移動->攻撃を行う
    /// </summary>
    private void AttackAtInterval()
    {
        _time += Time.deltaTime * GameManager.Instance.TimeController.EnemyTime;
        if (_time > Controller.Params.AttackRate && !_isApproaching)
        {
            _isApproaching = true;
            _shieldController.MoveForward();
            _shieldController.PlayAnimation(AnimationName.Move);
        }
        if (_time > Controller.Params.AttackRate + Controller.Params.AttackRange / Controller.Params.RunSpeed)
        {
            // 攻撃が1回きちんと行われることが確認できるまで2回目の攻撃が出ないような値に設定してある
            _time = NextAttackDelay;
            _isApproaching = false;
            _shieldController.CancelMoveToTarget();
            _shieldController.Attack();
            _shieldController.PlayAnimation(AnimationName.Attack);
        }
    }

    /// <summary>
    /// 弾を反射したらReflection状態に遷移する
    /// </summary>
    private bool TransitionReflection()
    {
        if (_shieldController.IsReflect)
        {
            _shieldController.LastStateType = StateType.AttackExtend;
            TryChangeState(StateType.Reflection);
            return true;
        }

        return false;
    }

    /// <summary>
    /// 視界から外れたらIdle状態に、攻撃範囲から外れたらMove状態に遷移する
    /// </summary>
    private bool Transition()
    {
        SightResult result = Controller.LookForPlayerInSight();
        if (result == SightResult.OutSight)
        {
            TryChangeState(StateType.IdleExtend);
            return true;
        }
        else if (result == SightResult.InSight)
        {
            TryChangeState(StateType.MoveExtend);
            return true;
        }

        return false;
    }
}
