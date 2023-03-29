using UnityEngine;

/// <summary>
/// �����~�܂��Ă����Ԃ̃N���X
/// ���Ԍo�߂�Search��ԂɑJ�ڂ���
/// </summary>
public class StateTypeIdle : StateTypeBase
{
    private float _delay;
    private float _time;

    public StateTypeIdle(EnemyController controller, StateType stateType)
        : base(controller, stateType) { }

    protected override void Enter()
    {
        Controller.PlayAnimation(AnimationName.Idle);

        // �����_���Ȏ��ԂőJ�ڂ���悤�ɐݒ肷��
        float min = Controller.Params.MinTransitionTimeElapsed;
        float max = Controller.Params.MaxTransitionTimeElapsed;
        _delay = Random.Range(min, max);
    }

    protected override void Stay()
    {
        SightResult result = Controller.IsFindPlayer();
        if (result == SightResult.InSight || result == SightResult.InAttackRange)
        {
            TryChangeState(StateType.Discover);
            return;
        }

        float timeScale = GameManager.Instance.TimeController.CurrentTimeScale.Value;
        _time += timeScale * Time.deltaTime;
        if (_time > _delay)
        {
            TryChangeState(StateType.Search);
        }
    }

    protected override void Exit()
    {
        _time = 0;
    }
}
