using UnityEngine;

/// <summary>
/// �v���C���[��T�����߂Ɉړ������Ԃ̃N���X
/// ���Ԍo�߂�Idle��ԂɑJ�ڂ���
/// </summary>
public class StateTypeSearch : StateTypeBase
{
    protected float _time;
    private int _cachedSEIndex;

    public StateTypeSearch(EnemyController controller, StateType stateType)
        : base(controller, stateType) { }

    protected override void Enter()
    {
        Controller.PlayAnimation(AnimationName.Search);
        Controller.MoveSeachForPlayer();

        _cachedSEIndex = GameManager.Instance.AudioManager.PlaySE("CueSheet_Gun", Controller.Params.WalkSEName);
    }

    protected override void Stay()
    {
        if (Controller.IsDefeated)
        {
            TryChangeState(StateType.Defeated);
            return;
        }

        SightResult result = Controller.LookForPlayerInSight();
        if (result == SightResult.InSight || result == SightResult.InAttackRange)
        {
            TryChangeState(StateType.Discover);
            return;
        }

        // �ړ����s�����\�b�h���Ăяo���Ď��Ԍo�߂�Idle�ɑJ�ڂ���
        // ���J��Ԃ��Ď��͂�T��������
        _time += Time.deltaTime * GameManager.Instance.TimeController.EnemyTime;
        float interval = Controller.Params.TurningPoint / Controller.Params.WalkSpeed;
        if (_time > interval)
        {
            _time = 0;
            TryChangeState(StateType.Idle);
        }
    }

    protected override void Exit()
    {
        _time = 0;
        Controller.CancelMoving();
        GameManager.Instance.AudioManager.StopSE(_cachedSEIndex);
    }
}
