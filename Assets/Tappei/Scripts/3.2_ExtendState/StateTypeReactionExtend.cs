/// <summary>
/// プレイヤーの銃撃に反応した際に遷移してくる状態
/// IdleExtendとSearchExtend状態から遷移してくる
/// 盾持ち用
/// </summary>
public class StateTypeReactionExtend : StateTypeMoveExtend
{
    public StateTypeReactionExtend(EnemyController controller, StateType stateType)
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
    /// 視界内/攻撃範囲内に入ったらDiscoverExtend状態に遷移する
    /// </summary>
    protected override bool Transition()
    {
        SightResult result = Controller.LookForPlayerInSight();
        if (result == SightResult.InSight || result == SightResult.InAttackRange)
        {
            TryChangeState(StateType.DiscoverExtend);
            return true;
        }

        return false;
    }
}
