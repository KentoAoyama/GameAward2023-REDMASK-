using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �G�̃X�e�[�g�}�V���Ŏg�p����e�X�e�[�g��o�^���Ă����N���X
/// </summary>
public class EnemyStateRegister
{
    private EnemyStateMachine _stateMachine;
    private EnemyStateMachineHelper _stateMachineHelper;
    private Dictionary<EnemyStateType, StateTypeBase> _stateDic = new();

    public EnemyStateRegister(EnemyStateMachine stateMachine, EnemyStateMachineHelper helper)
    {
        _stateMachine = stateMachine;
        _stateMachineHelper = helper;
    }

    /// <summary>
    /// ��肤��X�e�[�g�̎�ނ��w�肵�Đ����������^�ɓo�^����
    /// �o�^�����X�e�[�g��GetState()�ɂ���Ď擾�\
    /// </summary>
    public void Register(EnemyStateType type)
    {
        StateTypeBase state = CreateInstance(type);
        _stateDic.Add(type, state);
    }

    private StateTypeBase CreateInstance(EnemyStateType type)
    {
        Type stateClass = _stateMachineHelper.GetStateClassTypeWithEnum(type);
        object[] args = { _stateMachine };
        StateTypeBase instance = (StateTypeBase)Activator.CreateInstance(stateClass, args);

        return instance;
    }

    public StateTypeBase GetState(EnemyStateType type)
    {
        if (_stateDic.TryGetValue(type, out StateTypeBase state))
        {
            return state;
        }
        else
        {
            Debug.LogWarning("�Ή�����X�e�[�g���o�^����Ă��܂���: " + type);
            return null;
        }
    }
}
