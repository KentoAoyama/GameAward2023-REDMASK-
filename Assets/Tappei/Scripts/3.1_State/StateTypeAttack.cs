using UnityEngine;

/// <summary>
/// ˆê’èŠÔŠu‚ÅUŒ‚‚ğ‚·‚éó‘Ô‚ÌƒNƒ‰ƒX
/// </summary>
public class StateTypeAttack : StateTypeBase
{
    /// <summary>
    /// ‘JˆÚ‚ğŒJ‚è•Ô‚·‚±‚Æ‚Å‚Ì˜AË‘Îô‚Æ‚µ‚ÄA‚±‚Ì’l‚Íó‘Ô‚Ì‘JˆÚ‚ğ‚µ‚Ä‚à‰Šú‰»‚³‚ê‚È‚¢
    /// </summary>
    private float _time;

    public StateTypeAttack(EnemyController controller, StateType stateType)
        : base(controller, stateType) { }

    protected override void Enter()
    {
        Controller.PlayAnimation(AnimationName.Attack);
    }

    protected override void Stay()
    {
        if (TransitionDefeated()) return;
        AttackAtInterval();
        if (Transition()) return;
    }

    /// <summary>
    /// ˆê’èŠÔŠu‚ÅUŒ‚‚·‚é
    /// </summary>
    private void AttackAtInterval()
    {
        _time += Time.deltaTime * GameManager.Instance.TimeController.EnemyTime;
        if (_time > Controller.Params.AttackRate)
        {
            _time = 0;
            Controller.Attack();
        }
    }

    /// <summary>
    /// ‹ŠE‚©‚çŠO‚ê‚½‚çIdleó‘Ô‚ÉAUŒ‚”ÍˆÍ‚©‚çŠO‚ê‚½‚çMoveó‘Ô‚É‘JˆÚ‚·‚é
    /// </summary>
    private bool Transition()
    {
        SightResult result = Controller.LookForPlayerInSight();
        if (result == SightResult.OutSight)
        {
            TryChangeState(StateType.Idle);
            return true;
        }
        else if (result == SightResult.InSight)
        {
            TryChangeState(StateType.Move);
            return true;
        }

        return false;
    }
}
