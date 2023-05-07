using UnityEngine;

/// <summary>
/// �v���C���[�Ɍ����Ĉړ������Ԃ̃N���X
/// </summary>
public class StateTypeMove : StateTypeBase
{
    // �ȉ�2�͒n�ʂ̒[�Ȃǂňړ����L�����Z�����ꂽ�ꍇ��
    // ��莞�Ԍ�ɑJ�ڂ����鏈���ɕK�v�ȕϐ�
    private Vector3 _prevPos;
    private float _timer;
    private int _cachedSEIndex;

    public StateTypeMove(EnemyController controller, StateType stateType)
        : base(controller, stateType) { }

    protected override void Enter()
    {
        Controller.PlayAnimation(AnimationName.Move);
        Controller.MoveToPlayer();

        _prevPos = Vector3.one * -1000;
        _timer = 0;

        _cachedSEIndex = GameManager.Instance.AudioManager.PlaySE("CueSheet_Gun", Controller.Params.RunSEName);
    }

    protected override void Stay()
    {
        if (TransitionDefeated()) return;
        if (Transition()) return;
        if (TransitionAtMoveCancel()) return;
    }

    protected override void Exit()
    {
        Controller.CancelMoveToTarget();
        GameManager.Instance.AudioManager.StopSE(_cachedSEIndex);
    }

    /// <summary>
    /// �ړ����L�����Z�����ꂽ�ꍇ��Idle��ԂɑJ�ڂ���
    /// </summary>
    private bool TransitionAtMoveCancel()
    {
        if (IsMoveCancel())
        {
            TryChangeState(StateType.Idle);
            return true;
        }

        return false;
    }

    /// <summary>
    /// ���E����O�ꂽ��Idle��ԂɁA�U���͈͓��ɓ�������Attack��ԂɑJ�ڂ���
    /// </summary>
    private bool Transition()
    {
        SightResult result = Controller.LookForPlayerInSight();
        if (result == SightResult.OutSight)
        {
            TryChangeState(StateType.Idle);
            return true;
        }
        else if (result == SightResult.InAttackRange)
        {
            TryChangeState(StateType.Attack);
            return true;
        }

        return false;
    }

    /// <summary>
    /// �O�t���[������̈ړ��ʂ�0�̏�Ԃ���莞�ԑ����Ȃ�ړ����L�����Z�����ꂽ�Ƃ݂Ȃ�
    /// </summary>
    protected bool IsMoveCancel()
    {
        float distance = Vector3.Distance(_prevPos, Controller.transform.position);
        if (distance <= Mathf.Epsilon)
        {
            _timer += Time.deltaTime * GameManager.Instance.TimeController.EnemyTime;
        }
        else
        {
            _timer = 0;
        }
        _prevPos = Controller.transform.position;

        return _timer > EnemyParamsSO.MoveCancelTimeThreshold;
    }
}
