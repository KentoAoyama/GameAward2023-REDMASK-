/// <summary>
/// ���Ԋu�ōU���������Ԃ̃N���X
/// </summary>
public class StateTypeAttack : StateTypeBase
{
    public StateTypeAttack(EnemyController controller, StateType stateType)
        : base(controller, stateType) { }

    private float _interval;
    /// <summary>
    /// �J�ڂ��J��Ԃ����Ƃł̘A�ˑ΍�Ƃ���
    /// ���̒l�͏�Ԃ̑J�ڂ����Ă�����������Ȃ�
    /// </summary>
    private float _time;

    protected override void Enter()
    {
        _interval = Controller.Params.AttackRate;
    }

    protected override void Stay()
    {
        // TODO:�v���C���[�Ƃ͏�Ɉ�苗���ɂ��Ăق���

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
