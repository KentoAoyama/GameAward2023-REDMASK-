/// <summary>
/// �����~�܂��Ă���X�e�[�g�̃N���X
/// </summary>
public class StateTypeIdle : StateTypeBase
{
    public StateTypeIdle(BehaviorFacade facade, StateType stateType)
        : base(facade, stateType) { }

    protected override void Enter()
    {
        //Facade.SendMessage(BehaviorType.StopMove);
    }
}
