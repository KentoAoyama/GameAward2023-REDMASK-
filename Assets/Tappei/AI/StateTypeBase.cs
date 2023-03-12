using UnityEngine;

/// <summary>
/// �G�̃X�e�[�g�}�V���Ŏg�p����e�X�e�[�g�̊��N���X
/// �e�X�e�[�g�͕K�����̃N���X���p������K�v������
/// </summary>
public abstract class StateTypeBase
{
    private enum Stage
    {
        Enter,
        Stay,
        Exit,
    }

    private Stage _stage;
    private StateTypeBase _nextState;
    private EnemyStateMachine _stateMachine;

    public StateTypeBase(EnemyStateMachine stateMachine, StateType stateType)
    {
        StateType = stateType;
        _stateMachine = stateMachine;
    }

    public StateType StateType;

    /// <summary>1�x�̌Ăяo����Enter()/Stay()/Exit()�̂ǂꂩ1�����s�����</summary>
    public StateTypeBase Execute()
    {
        if (_stage == Stage.Enter)
        {
            Enter();
            _stage = Stage.Stay;
        }
        else if (_stage == Stage.Stay)
        {
            Stay();
        }
        else if (_stage == Stage.Exit)
        {
            Exit();
            _stage = Stage.Enter;

            return _nextState;
        }

        return this;
    }

    protected virtual void Enter() { }
    protected virtual void Stay() { }
    protected virtual void Exit() { }

    public virtual void Pause() { }
    public virtual void Resume() { }

    /// <summary>
    /// EnemyStateMachine����ǂ̃X�e�[�g�ɑJ�ڂ��邩���n�����
    /// EnemyStateMachine�̎���Update()��Exit()���Ă΂ꂽ��A�X�e�[�g���؂�ւ��
    /// Enter()���Ă΂��O�ɃX�e�[�g��؂�ւ��鎖�͏o���Ȃ�
    /// ���ɑJ�ڏ������Ă΂�Ă����ꍇ�͂��̑J�ڏ������L�����Z������
    /// </summary>
    public bool TryChangeState(StateTypeBase state)
    {
        if (_stage == Stage.Enter)
        {
            Debug.LogWarning("Enter()���Ă΂��O�ɃX�e�[�g��J�ڂ��邱�Ƃ͕s�\: " + state);
            return false;
        }
        else if (_stage == Stage.Exit)
        {
            Debug.LogWarning("���ɕʂ̃X�e�[�g�ɑJ�ڂ��鏈�����Ă΂�Ă��܂�: " + state);
            return false;
        }

        _stage = Stage.Exit;
        _nextState = state;

        return true;
    }
}