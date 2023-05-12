using UnityEngine;

/// <summary>
/// 盾持ち用
/// 一定間隔で攻撃をする状態のクラス
/// </summary>
public class StateTypeAttackExtend : StateTypeAttack
{
    private ShieldEnemyController _shieldController;

    /// <summary>
    /// 間合いを詰めてくる動作中かどうかのフラグ
    /// </summary>
    private bool _isApproaching;

    public StateTypeAttackExtend(EnemyController controller, StateType stateType)
    : base(controller, stateType) 
    {
        _shieldController = controller as ShieldEnemyController;
    }

    protected override void Stay()
    {
        if (TransitionDefeated()) return;
        if (TransitionReflection()) return;
        AttackAtInterval();
        Controller.DrawGuideline();
        if (Transition()) return;
    }

    protected override void Exit()
    {
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
            _time = 0;
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
