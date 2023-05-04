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
        Controller.Idle();

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
        _time += timeScale * Time.deltaTime;
        if (_time > _delay)
        {
            TryChangeState(Controller.IdleWhenUndiscover ? StateType.Idle : StateType.Search);
            return;
        }
    }

    protected override void Exit()
    {
        _time = 0;
    }
}
