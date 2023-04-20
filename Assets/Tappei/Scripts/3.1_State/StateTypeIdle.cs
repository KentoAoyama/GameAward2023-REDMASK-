using UnityEngine;

/// <summary>
/// —§‚¿~‚Ü‚Á‚Ä‚¢‚éó‘Ô‚ÌƒNƒ‰ƒX
/// ŠÔŒo‰ß‚ÅSearchó‘Ô‚É‘JˆÚ‚·‚é
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

        // ƒ‰ƒ“ƒ_ƒ€‚ÈŠÔ‚Å‘JˆÚ‚·‚é‚æ‚¤‚Éİ’è‚·‚é
        float min = Controller.Params.MinTransitionTimeElapsed;
        float max = Controller.Params.MaxTransitionTimeElapsed;
        _delay = Random.Range(min, max);
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
            if (Controller.Params.IsIdleUndiscovered)
            {
                TryChangeState(StateType.Idle);
            }
            else
            {
                TryChangeState(StateType.Search);
            }
            return;
        }
    }

    protected override void Exit()
    {
        _time = 0;
    }
}
