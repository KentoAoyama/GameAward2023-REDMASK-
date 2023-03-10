using UnityEngine;

/// <summary>
/// �G�̃X�e�[�g�}�V���Ŏg�p����e�X�e�[�g�̊��N���X
/// �e�X�e�[�g�͕K�����̃N���X���p������K�v������
/// </summary>
public abstract class EnemyStateBase
{
    private enum Stage
    {
        Enter,
        Stay,
        Exit,
    }

    private Stage _stage;
    private EnemyStateBase _nextState;
    private EnemyStateMachine _stateMachine;

    public EnemyStateBase(EnemyStateMachine stateMachine)
    {
        _stateMachine = stateMachine;
    }

    /// <summary>
    /// �X�e�[�g�̑J�ڏ������ĂԂƁA����EnemyStateMachine��Update()�̃^�C�~���O����
    /// ���̃X�e�[�g�ɐ؂�ւ��
    /// </summary>
    public EnemyStateBase Update()
    {
        if (_stage == Stage.Enter)
        {
            Enter();
            _stage = Stage.Stay;
        }
        if (_stage == Stage.Stay)
        {
            Stay();
        }
        if (_stage == Stage.Exit)
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

    private void ChangeState(EnemyStateType type)
    {
        _stage = Stage.Exit;
    }

    /// <summary>
    /// �C�ӂ̃X�e�[�g�ɑJ�ڂ��鏈������
    /// ���ɑJ�ڏ������Ă΂�Ă����ꍇ�͂��̑J�ڏ������L�����Z������
    /// </summary>
    protected bool TryChangeState(EnemyStateType type)
    {
        if (_stage == Stage.Stay)
        {
            ChangeState(type);
            return true;
        }
        else
        {
            Debug.LogWarning("���ɕʂ̃X�e�[�g�ɑJ�ڂ��鏈�����Ă΂�Ă��܂�: " + type);
            return false;
        }
    }
}
