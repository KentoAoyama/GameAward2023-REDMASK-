/// <summary>
/// ���j���ꂽ��Ԃ̃N���X
/// ����ȏ�͑J�ڂ��Ȃ�
/// </summary>
public class StateTypeDefeated : StateTypeBase
{
    public StateTypeDefeated(EnemyController controller, StateType stateType)
        : base(controller, stateType) { }

    protected override void Enter()
    {
        Controller.PlayAnimation(AnimationName.Dead);
    }
}