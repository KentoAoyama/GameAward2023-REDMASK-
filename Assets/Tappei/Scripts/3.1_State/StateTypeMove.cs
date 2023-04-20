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

    public StateTypeMove(EnemyController controller, StateType stateType)
        : base(controller, stateType) { }

    protected override void Enter()
    {
        Controller.PlayAnimation(AnimationName.Move);
        Controller.MoveToPlayer();

        _prevPos = Vector3.one * -1000;
        _timer = 0;
    }

    protected override void Stay()
    {
        if (Controller.IsDefeated)
        {
            TryChangeState(StateType.Defeated);
            return;
        }

        if (IsMoveCancel())
        {
            TryChangeState(StateType.Idle);
            return;
        }

        SightResult result = Controller.IsFindPlayer();
        if (result == SightResult.OutSight)
        {
            TryChangeState(StateType.Idle);
            return;
        }
        else if (result == SightResult.InAttackRange)
        {
            TryChangeState(StateType.Attack);
            return;
        }
    }

    protected override void Exit()
    {
        Controller.CancelMoving();
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

        return _timer > Controller.Params.MoveCancelTimerThreshold;
    }
}
