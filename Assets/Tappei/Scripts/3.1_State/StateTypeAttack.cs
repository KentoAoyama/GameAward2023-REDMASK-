/// <summary>
/// 一定間隔で攻撃をする状態のクラス
/// </summary>
public class StateTypeAttack : StateTypeBase
{
    protected float _interval;
    /// <summary>
    /// 遷移を繰り返すことでの連射対策として
    /// この値は状態の遷移をしても初期化されない
    /// </summary>
    protected float _time;

    public StateTypeAttack(EnemyController controller, StateType stateType)
        : base(controller, stateType) { }

    protected override void Enter()
    {
        Controller.PlayAnimation(AnimationName.Attack);

        _interval = Controller.Params.AttackRate;
        //Controller.MoveToPlayer();
    }

    protected override void Stay()
    {
        // TODO:プレイヤーとは常に一定距離にいてほしい

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
            TryChangeState(StateType.Search);
            return;
        }
        else if (result == SightResult.InSight)
        {
            TryChangeState(StateType.Move);
            return;
        }
    }
}
