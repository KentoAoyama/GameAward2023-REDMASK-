using UnityEngine;

/// <summary>
/// StateRegister�N���X���琶������̂ŁA�p�������X�e�[�g��
/// �R���X�g���N�^�͂��̃X�e�[�g�Ɠ����ł���K�v������
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

    public StateTypeBase(EnemyController controller, StateType type)
    {
        Controller = controller;
        Type = type;
    }

    protected EnemyController Controller { get; }
    public StateType Type { get; }

    /// <summary>
    /// 1�x�̌Ăяo���ŃX�e�[�g�̒i�K�ɉ�����Enter() Stay() Exit()�̂����ǂꂩ1�����s�����
    /// </summary>
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

    /// <summary>
    /// ���j���ꂽ�ꍇ�̏�����Defeated��ԈȊO�͋��ʏ����Ȃ̂Ŋ��N���X�Ɏ������Ă���
    /// �J�ڂ���ꍇ��true���Ԃ�
    /// </summary>
    protected bool TransitionDefeated()
    {
        if (Controller.IsDefeated)
        {
            TryChangeState(StateType.Defeated);
            return true;
        }

        return false;
    }

    public virtual void OnPause() { }
    public virtual void OnResume() { }
    public virtual void OnDisable() { }

    /// <summary>
    /// Enter()���Ă΂�Ă��A�X�e�[�g�̑J�ڏ������Ă�ł��Ȃ��ꍇ�̂ݑJ�ډ\
    /// </summary>
    public bool TryChangeState(StateType type)
    {
        StateTypeBase state = Controller.GetState(type);

        if (_stage == Stage.Enter)
        {
            Debug.LogWarning("Enter()���Ă΂��O�ɃX�e�[�g��J�ڂ��邱�Ƃ͕s�\: �J�ڐ�: " + state);
            return false;
        }
        else if (_stage == Stage.Exit)
        {
            Debug.LogWarning("���ɕʂ̃X�e�[�g�ɑJ�ڂ��鏈�����Ă΂�Ă��܂�: �J�ڐ�: " + state);
            return false;
        }

        _stage = Stage.Exit;
        _nextState = state;

        return true;
    }
}