using UnityEngine;

/// <summary>
/// 盾持ち用
/// 立ち止まっている状態のクラス
/// 時間経過でSearch状態に遷移する
/// </summary>
public class StateTypeIdleExtend : StateTypeIdle
{
    private ShieldEnemyController _shieldController;

    public StateTypeIdleExtend(EnemyController controller, StateType stateType)
    : base(controller, stateType)
    {
        _shieldController = controller as ShieldEnemyController;
    }

    protected override void Stay()
    {
        _shieldController.UpdateIdle();

        if (TransitionDefeated()) return;
        if (TransitionReflection()) return;
        if (Transition()) return;
        if (TransitionAtTimeElapsed()) return;
    }

    /// <summary>
    /// 弾を反射したらReflection状態に遷移する
    /// </summary>
    private bool TransitionReflection()
    {
        if (_shieldController.IsReflect)
        {
            _shieldController.LastStateType = StateType.IdleExtend;
            TryChangeState(StateType.Reflection);
            return true;
        }

        return false;
    }

    /// <summary>
    /// 視界内/攻撃範囲内に入ったらDiscover状態に遷移する
    /// </summary>
    private bool Transition()
    {
        SightResult result = Controller.LookForPlayerInSight();
        if (result == SightResult.InSight || result == SightResult.InAttackRange)
        {
            TryChangeState(StateType.DiscoverExtend);
            return true;
        }

        return false;
    }

    /// <summary>
    /// 視界内/攻撃範囲内に入ったらDiscover状態に遷移する
    /// </summary>
    private bool TransitionAtTimeElapsed()
    {
        _time += Time.deltaTime * GameManager.Instance.TimeController.EnemyTime;
        if (_time > _delay)
        {
            TryChangeState(Controller.IdleWhenUndiscover ? StateType.IdleExtend : StateType.SearchExtend);
            return true;
        }

        return false;
    }
}
