/// <summary>
/// ���Ԋu�ōU��������X�e�[�g�̃N���X
/// </summary>
public class StateTypeAttack : StateTypeBase
{
    // TODO:�{���Ȃ�U���Ԋu�͊O������ݒ�o����Ɨǂ�
    private static readonly float Interval = 120.0f;

    // TODO:���̃^�C���X�s�[�h�A���̒l�𑀍삷�邱�ƂŃX���[���[�V����/�|�[�Y�ɑΉ�������
    private float _timeSpeed = 1.0f;
    private float _timer;

    public StateTypeAttack(BehaviorMessenger messenger, StateType stateType)
        : base(messenger, stateType) { }

    protected override void Enter()
    {
        _timer = 0;
        _messenger.SendMessage(BehaviorType.Attack);
    }

    protected override void Stay()
    {
        _timer += _timeSpeed;
        if (_timer > Interval)
        {
            _timer = 0;
            _messenger.SendMessage(BehaviorType.Attack);
        }
    }
}
