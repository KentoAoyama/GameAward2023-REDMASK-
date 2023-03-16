/// <summary>
/// 一定間隔で攻撃をするステートのクラス
/// </summary>
public class StateTypeAttack : StateTypeBase
{
    // TODO:本来なら攻撃間隔は外部から設定出来ると良い
    private static readonly float Interval = 120.0f;

    // TODO:仮のタイムスピード、この値を操作することでスローモーション/ポーズに対応させる
    private float _timeSpeed = 1.0f;
    private float _timer;

    public StateTypeAttack(BehaviorMessenger messenger, StateType stateType)
        : base(messenger, stateType) { }

    protected override void Enter()
    {
        _timer = 0;
        _messenger.SendMessage(BehaviorType.Attack);
    }

    protected override void Stay()
    {
        _timer += _timeSpeed;
        if (_timer > Interval)
        {
            _timer = 0;
            _messenger.SendMessage(BehaviorType.Attack);
        }
    }
}
