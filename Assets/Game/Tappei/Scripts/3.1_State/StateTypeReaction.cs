using UnityEngine;

/// <summary>
/// プレイヤーの銃撃に反応した際に遷移してくる状態
/// IdleとSearch状態から遷移してくる
/// </summary>
public class StateTypeReaction : StateTypeMove
{
    public StateTypeReaction(EnemyController controller, StateType stateType)
        : base(controller, stateType) { }

    protected override void Enter()
    {
        Controller.PlayAnimation(AnimationName.Move);
        Controller.MoveToPlayerLastPos();
        ResetOnEnter();
    }

    protected override void Exit()
    {
        base.Exit();
        // Controller側のフラグを折ることで
        // もう一度メッセージを受信した際にこの状態に遷移できるようにする
        Controller.IsReaction = false;
    }

    /// <summary>
    /// 視界内/攻撃範囲内に入ったらDiscover状態に遷移する
    /// </summary>
    protected override bool Transition()
    {
        SightResult result = Controller.LookForPlayerInSight();
        if (result == SightResult.InSight || result == SightResult.InAttackRange)
        {
            TryChangeState(StateType.Discover);
            return true;
        }

        return false;
    }

    #region 未使用
    /// <summary>
    /// 到着していたらIdle状態に遷移する
    /// </summary>
    private bool TransitionAtArrival()
    {
        float distance = Vector3.Distance(Controller.transform.position, Controller.PlayerLastPos);
        if (distance <= Mathf.Epsilon)
        {
            TryChangeState(StateType.Idle);
            return true;
        }

        return false;
    }
    #endregion
}
