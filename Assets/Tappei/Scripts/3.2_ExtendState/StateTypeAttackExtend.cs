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
    /// 間合いを詰めてくる動作中かどうかのフラグ
    /// </summary>
    private bool _isApproaching;

    public StateTypeAttackExtend(EnemyController controller, StateType stateType)
    : base(controller, stateType) 
    {
        _shieldController = controller as ShieldEnemyController;
    }

    protected override void Enter()
    {
        _time = Controller.Params.AttackRate;
        _shieldController.PlayAnimation(AnimationName.Idle);
    }

    protected override void Stay()
    {
        if (Controller.IsDefeated)
        {
            TryChangeState(StateType.Defeated);
            return;
        }

        if (_shieldController.IsReflect)
        {
            _shieldController.LastStateType = StateType.AttackExtend;
            TryChangeState(StateType.Reflection);
            return;
        }

        _time += Time.deltaTime * GameManager.Instance.TimeController.EnemyTime * AttackTimerMag;
        if (_time > Controller.Params.AttackRate && !_isApproaching)
        {
            _isApproaching = true;
            _shieldController.MoveForward();
            _shieldController.PlayAnimation(AnimationName.Move);
        }
        if(_time > Controller.Params.AttackRate + MovingDistanceMag * Controller.Params.AttackRange)
        {
            _time = NextAttackDelay;
            _isApproaching = false;
            _shieldController.CancelMoving();
            _shieldController.Attack();
            _shieldController.PlayAnimation(AnimationName.Attack);
        }

        SightResult result = Controller.IsFindPlayer();
        if (result == SightResult.OutSight)
        {
            TryChangeState(StateType.IdleExtend);
            return;
        }
        else if (result == SightResult.InSight)
        {
            TryChangeState(StateType.MoveExtend);
            return;
        }
    }

    protected override void Exit()
    {
        _time = 0;
        _isApproaching = false;
    }
}
