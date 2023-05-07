using UnityEngine;

/// <summary>
/// 一定間隔で攻撃をする状態のクラス
/// </summary>
public class StateTypeAttack : StateTypeBase
{
    /// <summary>
    /// 攻撃するまでのタイマーのカウントの倍率
    /// deltaTimeとの乗算で攻撃までの間隔を調整する
    /// </summary>
    protected static readonly int AttackTimerMag = 60;
    /// <summary>
    /// 遷移を繰り返すことでの連射対策として
    /// StateTypeAttackクラスではこの値は状態の遷移をしても初期化されない
    /// </summary>
    protected float _time;

    public StateTypeAttack(EnemyController controller, StateType stateType)
        : base(controller, stateType) { }

    protected override void Enter()
    {
        Controller.PlayAnimation(AnimationName.Attack);
        //Controller.MoveToPlayer();
    }

    protected override void Stay()
    {
        _time += Time.deltaTime * GameManager.Instance.TimeController.EnemyTime * AttackTimerMag;
        if (_time > Controller.Params.AttackRate)
        {
            _time = 0;
            Controller.Attack();
        }

        if (Controller.IsDefeated)
        {
            TryChangeState(StateType.Defeated);
            return;
        }

        SightResult result = Controller.LookForPlayerInSight();
        if (result == SightResult.OutSight)
        {
            TryChangeState(StateType.Idle);
            return;
        }
        else if (result == SightResult.InSight)
        {
            TryChangeState(StateType.Move);
            return;
        }
    }
}
