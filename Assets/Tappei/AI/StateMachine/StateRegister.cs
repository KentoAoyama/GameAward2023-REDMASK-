using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �G�̃X�e�[�g�}�V���Ŏg�p����e�X�e�[�g��o�^���Ă����N���X
/// �X�e�[�g�}�V�������g�̏��������ɂ��̃N���X��p���Ďg�p����X�e�[�g�̐����Ɠo�^���s��
/// </summary>
public class StateRegister
{
    /// <summary>
    /// StateType��1:1�őΉ������X�e�[�g����������̂�
    /// �X�e�[�g�̐����������e�ʂ��m�ۂ��Ă���
    /// </summary>
    private static readonly int StateDicCap = Enum.GetValues(typeof(StateType)).Length;

    private BehaviorMessenger _messenger;
    private StateMachineHelper _stateMachineHelper;
    private Dictionary<StateType, StateTypeBase> _stateDic = new(StateDicCap);

    public StateRegister(BehaviorMessenger messenger, StateMachineHelper helper)
    {
        _messenger = messenger;
        _stateMachineHelper = helper;
    }

    /// <summary>
    /// ��肤��X�e�[�g�̎�ނ��w�肵�Đ����������^�ɓo�^����
    /// �o�^�����X�e�[�g��GetState()�ɂ���Ď擾�\
    /// </summary>
    public void Register(StateType type)
    {
        if (_stateDic.ContainsKey(type))
        {
            Debug.LogWarning("�����ɃX�e�[�g�����ɓo�^����Ă��܂�: " + type);
            return;
        }

        StateTypeBase state = CreateInstance(type);
        _stateDic.Add(type, state);
    }

    private StateTypeBase CreateInstance(StateType type)
    {
        Type stateClass = _stateMachineHelper.GetStateClassType(type);

        if (stateClass == null)
        {
            Debug.LogError("StateType�ɃX�e�[�g���R�Â����Ă��܂���: " + type);;
            return null;
        }

        object[] args = { _messenger, type };
        StateTypeBase instance = (StateTypeBase)Activator.CreateInstance(stateClass, args);

        return instance;
    }

    public StateTypeBase GetState(StateType type)
    {
        if (_stateDic.TryGetValue(type, out StateTypeBase state))
        {
            return state;
        }
        else
        {
            Debug.LogError("�Ή�����X�e�[�g�������ɓo�^����Ă��܂���: " + type);
            return null;
        }
    }
}
