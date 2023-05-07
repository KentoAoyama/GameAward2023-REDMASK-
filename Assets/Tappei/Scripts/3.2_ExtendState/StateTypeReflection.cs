using UnityEngine;

/// <summary>
/// ‚‚¿—p
/// ‚‚ÉUŒ‚‚ğó‚¯‚Ä‚µ‚Î‚ç‚­d’¼‚µ‚Ä‚¢‚éó‘Ô
/// </summary>
public class StateTypeReflection : StateTypeBase
{
    private ShieldEnemyController _shieldController;
    private float _delay;
    private float _time;
    private bool _isPostured;

    public StateTypeReflection(EnemyController controller, StateType stateType)
    : base(controller, stateType)
    {
        _shieldController = controller as ShieldEnemyController;
    }

    protected override void Enter()
    {
        _delay = _shieldController.ShieldParams.StiffeningTime;
        _shieldController.PlayAnimation(AnimationName.Reflection);

        GameManager.Instance.AudioManager.PlaySE("CueSheet_Gun", "SE_Enemy_Damage_Shield");
    }

    protected override void Stay()
    {
        if (Controller.IsDefeated)
        {
            TryChangeState(StateType.Defeated);
            return;
        }

        float timeScale = GameManager.Instance.TimeController.EnemyTime;
        _time += timeScale * Time.deltaTime;
        if (_time > _delay - 1.0f && !_isPostured)
        {
            _isPostured = true;
            Controller.PlayAnimation(AnimationName.Posture);
        }
        else if (_time > _delay)
        {
            TryChangeState(_shieldController.LastStateType);
        }
    }

    protected override void Exit()
    {
        _time = 0;
        _delay = 0;
        _isPostured = false;
        _shieldController.RecoverShield();
    }
}
