using UnityEngine;

/// <summary>
/// 盾持ち用
/// プレイヤーを探すために移動する状態のクラス
/// 時間経過でIdle状態に遷移する
/// </summary>
public class StateTypeSearchExtend : StateTypeSearch
{
    private ShieldEnemyController _shieldController;

    public StateTypeSearchExtend(EnemyController controller, StateType stateType)
    : base(controller, stateType) 
    {
        _shieldController = controller as ShieldEnemyController;
    }

    protected override void Stay()
    {
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
            _shieldController.LastStateType = StateType.SearchExtend;
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
    /// 一定時間が経過したらIdle状態に遷移する
    /// </summary>
    private bool TransitionAtTimeElapsed()
    {
        _time += Time.deltaTime * GameManager.Instance.TimeController.EnemyTime;
        float interval = Controller.Params.TurningPoint / Controller.Params.WalkSpeed;
        if (_time > interval)
        {
            TryChangeState(StateType.IdleExtend);
            return true;
        }

        return false;
    }
}
