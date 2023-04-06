/// <summary>
/// 盾持ち用
/// 一定間隔で攻撃をする状態のクラス
/// </summary>
public class StateTypeAttackExtend : StateTypeAttack
{
    private ShieldEnemyController _shieldController;

    public StateTypeAttackExtend(EnemyController controller, StateType stateType)
    : base(controller, stateType) 
    {
        _shieldController = controller as ShieldEnemyController;
    }

    protected override void Stay()
    {
        // TODO:プレイヤーとは常に一定距離にいてほしい

        if (Controller.IsDefeated)
        {
            TryChangeState(StateType.Defeated);
            return;
        }

        if (_shieldController.IsReflect)
        {
            _shieldController.LastStateType = StateType.AttackExtend;
            TryChangeState(StateType.Reflection);
            return;
        }

        float timeScale = GameManager.Instance.TimeController.EnemyTime;
        _time += timeScale;
        if (_time > _interval)
        {
            _time = 0;
            Controller.Attack();
        }

        SightResult result = Controller.IsFindPlayer();
        if (result == SightResult.OutSight)
        {
            TryChangeState(StateType.SearchExtend);
            return;
        }
        else if (result == SightResult.InSight)
        {
            TryChangeState(StateType.MoveExtend);
            return;
        }
    }
}
