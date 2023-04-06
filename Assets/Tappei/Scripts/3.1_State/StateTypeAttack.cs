/// <summary>
/// ���Ԋu�ōU���������Ԃ̃N���X
/// </summary>
public class StateTypeAttack : StateTypeBase
{
    protected float _interval;
    /// <summary>
    /// �J�ڂ��J��Ԃ����Ƃł̘A�ˑ΍�Ƃ���
    /// ���̒l�͏�Ԃ̑J�ڂ����Ă�����������Ȃ�
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
        // TODO:�v���C���[�Ƃ͏�Ɉ�苗���ɂ��Ăق���

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
