/// <summary>
/// �v���C���[��T�����߂Ɉړ�����X�e�[�g�̃N���X
/// </summary>
public class StateTypeSearch : StateTypeBase
{
    // TODO:���낤��̋������L�������菬���������肷��Ƃ��̒l�������ł����낤��Ԋu���ς���Ă��܂�
    //      �^�C���X�s�[�h��������̂Ń|�[�Y/�X���[���[�V�����͋C�ɂ��Ȃ��ł悢
    //private static readonly float Interval = 240.0f;

    // TODO:���̃^�C���X�s�[�h�A���̒l�𑀍삷�邱�ƂŃX���[���[�V����/�|�[�Y�ɑΉ�������
    private float _timeSpeed = 1.0f;
    private float _timer;

    public StateTypeSearch(BehaviorFacade facade, StateType stateType)
        : base(facade, stateType) { }

    protected override void Enter()
    {
        _timer = 0;
        //Facade.SendMessage(BehaviorType.SearchMove);
    }

    protected override void Stay()
    {        
        //_timer += _timeSpeed;
        //if (_timer > Interval)
        //{
        //    _timer = 0;
        //    Facade.SendMessage(BehaviorType.SearchMove);
        //}
    }
}
