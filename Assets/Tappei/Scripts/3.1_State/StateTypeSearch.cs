/// <summary>
/// プレイヤーを探すために移動する状態のクラス
/// 時間経過でIdle状態に遷移する
/// </summary>
public class StateTypeSearch : StateTypeBase
{
    /// <summary>遷移するまでの間隔を調整するために使用する値</summary>
    private static readonly int TransitionTimerMag = 60; 

    protected float _interval;
    protected float _time;

    public StateTypeSearch(EnemyController controller, StateType stateType)
        : base(controller, stateType) { }

    protected override void Enter()
    {
        Controller.PlayAnimation(AnimationName.Search);

        // 移動を行うメソッドを呼び出して時間経過でIdleに遷移する
        // を繰り返して周囲を探索させる
        _interval = Controller.Params.TurningPoint / Controller.Params.WalkSpeed * TransitionTimerMag;
        Controller.SearchMoving();
    }

    protected override void Stay()
    {
        if (Controller.IsDefeated)
        {
            TryChangeState(StateType.Defeated);
            return;
        }

        SightResult result = Controller.IsFindPlayer();
        if (result == SightResult.InSight || result == SightResult.InAttackRange)
        {
            TryChangeState(StateType.Discover);
            return;
        }

        float timeScale = GameManager.Instance.TimeController.EnemyTime;
        _time += timeScale;
        if (_time > _interval)
        {
            _time = 0;
            TryChangeState(StateType.Idle);
        }
    }

    protected override void Exit()
    {
        _time = 0;
        Controller.CancelMoving();
    }
}
