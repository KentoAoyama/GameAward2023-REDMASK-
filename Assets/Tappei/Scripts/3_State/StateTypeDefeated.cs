/// <summary>
/// ���j���ꂽ�ۂ̃X�e�[�g�̃N���X
/// </summary>
public class StateTypeDefeated : StateTypeBase
{
    public StateTypeDefeated(BehaviorFacade facade, StateType stateType)
        : base(facade, stateType) { }

    protected override void Enter()
    {
        //Facade.SendMessage(BehaviorType.Defeated);
    }
}
