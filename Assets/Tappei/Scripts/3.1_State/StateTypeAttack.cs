using UnityEngine;

/// <summary>
/// ˆê’èŠÔŠu‚ÅUŒ‚‚ğ‚·‚éó‘Ô‚ÌƒNƒ‰ƒX
/// </summary>
public class StateTypeAttack : StateTypeBase
{
    /// <summary>‘JˆÚ‚·‚é‚Ü‚Å‚ÌŠÔŠu‚ğ’²®‚·‚é‚½‚ß‚Ég—p‚·‚é’l</summary>
    protected static readonly int AttackTimerMag = 60;
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
        //Controller.MoveToPlayer();
    }

    protected override void Stay()
    {
        _time += Time.deltaTime * GameManager.Instance.TimeController.EnemyTime * AttackTimerMag;
        if (_time > Controller.Params.AttackRate)
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
