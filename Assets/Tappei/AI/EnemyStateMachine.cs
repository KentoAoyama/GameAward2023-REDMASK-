using UniRx;
using UnityEngine;

/// <summary>
/// �G�̃X�e�[�g�}�V��
/// </summary>
[RequireComponent(typeof(StateTransitionMessageReceiver))]
public class EnemyStateMachine : MonoBehaviour, IPausable
{
    [SerializeField] private StateTransitionMessageReceiver _messageReceiver;

    private ReactiveProperty<StateTypeBase> _currentState = new();
    private EnemyStateRegister _stateRegister;

    public IReadOnlyReactiveProperty<StateTypeBase> CurrentState;

    private void Awake()
    {
        EnemyStateMachineHelper helper = new();
        // TODO:�X�e�[�g�}�V�����n���Ă��邪�A�X�e�[�g���ŕs�v�Ȃ�n���Ȃ��Ă悢
        _stateRegister = new(this, helper);
        _stateRegister.Register(EnemyStateType.Idle);
        _stateRegister.Register(EnemyStateType.Search);

        _currentState.Value = _stateRegister.GetState(EnemyStateType.Idle);
    }

    void Start()
    {
        // ��ԑJ�ڂɎg�����́A�����J�ڂ݂̂������āA�J�ڂ��邩��Ԃ��N���X��p�ӂ���
        // �v���C���[�𔭌�����
        // �v���C���[����������
        // �_���[�W���󂯂�
        // �̗͂����ȉ��ɂȂ���
        // ��莞�Ԃ�������

        // Idle
        // ��莞�Ԍo�߁cSearch�ɑJ��
        // �v���C���[�����cMove�ɑJ��
        
        // Search
        // ��莞�Ԍo�߁cIdle�ɑJ��
        // �v���C���[�����cMove�ɑJ��

        // Move
        // �v���C���[�Ƃ̋������U���͈͓��cAttack�ɑJ��
        // �v���C���[�����������cSearch�ɑJ��

        // Attack
        // �v���C���[�Ƃ̋������U���͈͊O�cMove�ɑJ��
        // �v���C���[�����������cSearch�ɑJ��

        // �e�X�e�[�g�ɑ΂���
        // ����n����������X�e�[�gs�ɑJ�ڂ���
        // �Ƃ������̂������ق���
        // �����ƑJ�ڐ�̓y�A
    }

    void Update()
    {
        _currentState.Value = _currentState.Value.Execute();

        // �J�ڏ������������ōs��
        // �X�e�[�g�̏������I�������J�ڂ̔�������Ă���
        //StateTypeBase nextState = _stateRegister.GetState(EnemyStateType.Search);
        //_currentState.Value.TryChangeState(nextState);
    }

    public void Pause() => _currentState.Value.Pause();
    public void Resume() => _currentState.Value.Resume();
}