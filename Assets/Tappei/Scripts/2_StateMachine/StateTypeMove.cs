/// <summary>
/// �v���C���[�Ɍ����Ĉړ�����X�e�[�g�̃N���X
/// </summary>
public class StateTypeMove : StateTypeBase
{
    public StateTypeMove(BehaviorMessenger messenger, StateType stateType)
        : base(messenger, stateType) { }

    protected override void Enter()
    {
        _messenger.SendMessage(BehaviorType.MoveToPlayer);
    }

    protected override void Exit()
    {
        _messenger.SendMessage(BehaviorType.StopMove);
    }
}
