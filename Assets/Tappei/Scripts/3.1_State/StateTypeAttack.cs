using UnityEngine;

/// <summary>
/// 一定間隔で攻撃をする状態のクラス
/// </summary>
public class StateTypeAttack : StateTypeBase
{
    /// <summary>
    /// 遷移を繰り返すことでの連射対策として、この値は状態の遷移をしても初期化されない
    /// </summary>
    private float _time;

    public StateTypeAttack(EnemyController controller, StateType stateType)
        : base(controller, stateType) { }

    protected override void Enter()
    {
        Controller.PlayAnimation(AnimationName.Attack);
    }

    protected override void Stay()
    {
        if (TransitionDefeated()) return;
        AttackAtInterval();
        if (Transition()) return;
    }

    /// <summary>
    /// 一定間隔で攻撃する
    /// </summary>
    private void AttackAtInterval()
    {
        _time += Time.deltaTime * GameManager.Instance.TimeController.EnemyTime;
        if (_time > Controller.Params.AttackRate)
        {
            _time = 0;
            Controller.Attack();
        }
    }

    /// <summary>
    /// 視界から外れたらIdle状態に、攻撃範囲から外れたらMove状態に遷移する
    /// </summary>
    private bool Transition()
    {
        SightResult result = Controller.LookForPlayerInSight();
        if (result == SightResult.OutSight)
        {
            TryChangeState(StateType.Idle);
            return true;
        }
        else if (result == SightResult.InSight)
        {
            TryChangeState(StateType.Move);
            return true;
        }

        return false;
    }
}
