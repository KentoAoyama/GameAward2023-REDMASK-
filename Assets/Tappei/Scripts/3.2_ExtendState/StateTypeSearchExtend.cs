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
        if (_shieldController.IsReflect)
        {
            _shieldController.LastStateType = StateType.SearchExtend;
            TryChangeState(StateType.Reflection);
            return;
        }

        SightResult result = Controller.IsFindPlayer();
        if (result == SightResult.InSight || result == SightResult.InAttackRange)
        {
            TryChangeState(StateType.DiscoverExtend);
            return;
        }

        float timeScale = GameManager.Instance.TimeController.EnemyTime;
        _time += timeScale;
        if (_time > _interval)
        {
            _time = 0;
            TryChangeState(StateType.IdleExtend);
        }
    }
}
