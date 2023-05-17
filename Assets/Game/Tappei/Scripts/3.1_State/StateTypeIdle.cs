using System;
using UnityEngine;

/// <summary>
/// 立ち止まっている状態のクラス
/// 時間経過でSearch状態に遷移する
/// </summary>
public class StateTypeIdle : StateTypeBase
{
    protected float _delay;
    protected float _time;

    public StateTypeIdle(EnemyController controller, StateType stateType)
        : base(controller, stateType) { }

    protected override void Enter()
    {
        Controller.PlayAnimation(AnimationName.Idle);

        // ランダムな時間で遷移するように設定する
        _delay = Controller.Params.GetRandomIdleStateTimer();
    }

    protected override void Stay()
    {
        Controller.UpdateIdle();

        if (TransitionDefeated()) return;
        if (TransitionAtReaction()) return;
        if (Transition()) return;
        if (TransitionAtTimeElapsed()) return;
    }

    protected override void Exit()
    {
        _time = 0;
    }

    private bool TransitionAtReaction()
    {
        if (Controller.IsReaction)
        {
            TryChangeState(StateType.Reaction);
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
            TryChangeState(StateType.Discover);
            return true;
        }

        return false;
    }

    /// <summary>
    /// 時間経過でIdleもしくはSearch状態に遷移する
    /// </summary>
    private bool TransitionAtTimeElapsed()
    {
        _time += Time.deltaTime * GameManager.Instance.TimeController.EnemyTime;
        if (_time > _delay)
        {
            TryChangeState(Controller.IdleWhenUndiscover ? StateType.Idle : StateType.Search);
            return true;
        }

        return false;
    }
}
