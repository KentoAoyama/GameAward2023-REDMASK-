/// <summary>
/// プレイヤーを探すために移動するステートのクラス
/// </summary>
public class StateTypeSearch : StateTypeBase
{
    // TODO:うろうろの距離を広くしたり小さくしたりするとこの値が同じでもうろうろ間隔が変わってしまう
    //      タイムスピードをかけるのでポーズ/スローモーションは気にしないでよい
    //private static readonly float Interval = 240.0f;

    // TODO:仮のタイムスピード、この値を操作することでスローモーション/ポーズに対応させる
    private float _timeSpeed = 1.0f;
    private float _timer;

    public StateTypeSearch(BehaviorFacade facade, StateType stateType)
        : base(facade, stateType) { }

    protected override void Enter()
    {
        _timer = 0;
        //Facade.SendMessage(BehaviorType.SearchMove);
    }

    protected override void Stay()
    {        
        //_timer += _timeSpeed;
        //if (_timer > Interval)
        //{
        //    _timer = 0;
        //    Facade.SendMessage(BehaviorType.SearchMove);
        //}
    }
}
