/// <summary>
/// ˆê’èŠÔŠu‚ÅUŒ‚‚ğ‚·‚éó‘Ô‚ÌƒNƒ‰ƒX
/// </summary>
public class StateTypeAttack : StateTypeBase
{
    protected float _interval;
    /// <summary>
    /// ‘JˆÚ‚ğŒJ‚è•Ô‚·‚±‚Æ‚Å‚Ì˜AË‘Îô‚Æ‚µ‚Ä
    /// ‚±‚Ì’l‚Íó‘Ô‚Ì‘JˆÚ‚ğ‚µ‚Ä‚à‰Šú‰»‚³‚ê‚È‚¢
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
        // TODO:ƒvƒŒƒCƒ„[‚Æ‚Íí‚Éˆê’è‹——£‚É‚¢‚Ä‚Ù‚µ‚¢

        float timeScale = GameManager.Instance.TimeController.EnemyTime;
        _time += timeScale;
        if (_time > _interval)
        {
            _time = 0;
            Controller.Attack();
        }

        if (Controller.IsDefeated)
        {
            TryChangeState(StateType.Defeated);
            return;
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
