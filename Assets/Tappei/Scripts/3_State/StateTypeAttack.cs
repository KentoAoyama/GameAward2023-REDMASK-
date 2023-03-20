/// <summary>
/// ˆê’èŠÔŠu‚ÅUŒ‚‚ğ‚·‚éó‘Ô‚ÌƒNƒ‰ƒX
/// </summary>
public class StateTypeAttack : StateTypeBase
{
    public StateTypeAttack(EnemyController controller, StateType stateType)
        : base(controller, stateType) { }

    private float _interval;
    /// <summary>
    /// ‘JˆÚ‚ğŒJ‚è•Ô‚·‚±‚Æ‚Å‚Ì˜AË‘Îô‚Æ‚µ‚Ä
    /// ‚±‚Ì’l‚Íó‘Ô‚Ì‘JˆÚ‚ğ‚µ‚Ä‚à‰Šú‰»‚³‚ê‚È‚¢
    /// </summary>
    private float _time;

    protected override void Enter()
    {
        _interval = Controller.Params.AttackRate;
    }

    protected override void Stay()
    {
        // TODO:ƒvƒŒƒCƒ„[‚Æ‚Íí‚Éˆê’è‹——£‚É‚¢‚Ä‚Ù‚µ‚¢

        float timeScale = GameManager.Instance.TimeController.CurrentTimeScale.Value;
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
