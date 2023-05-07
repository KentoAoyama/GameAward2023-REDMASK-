using UnityEngine;

/// <summary>
/// �������p
/// ���ɍU�����󂯂Ă��΂炭�d�����Ă�����
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
        if (TransitionDefeated()) return;
        if (RecoverProcess()) return;
    }

    protected override void Exit()
    {
        _time = 0;
        _delay = 0;
        _isPostured = false;
        _shieldController.RecoverShield();
    }

    /// <summary>
    /// ���Ԍo�߂ŏ����\�������A�j���[�V�������Đ�����
    /// �A�j���[�V�����̍Đ���A�Ō�̏�ԂɑJ�ڂ���
    /// </summary>
    private bool RecoverProcess()
    {
        _time += Time.deltaTime * GameManager.Instance.TimeController.EnemyTime;
        if (_time > _delay - 1.0f && !_isPostured)
        {
            _isPostured = true;
            Controller.PlayAnimation(AnimationName.Posture);
        }
        else if (_time > _delay)
        {
            TryChangeState(_shieldController.LastStateType);
            return true;
        }

        return false;
    }
}
