using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachineStateRegister
{
    private EnemyStateMachine _stateMachine;
    private EnemyStateMachineHelper _stateMachineHelper;
    private Dictionary<EnemyStateType, EnemyStateBase> _stateDic = new();

    public EnemyStateMachineStateRegister(EnemyStateMachine stateMachine, EnemyStateMachineHelper helper)
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
        EnemyStateBase state = CreateInstance(type);
        _stateDic.Add(type, state);
    }

    private EnemyStateBase CreateInstance(EnemyStateType type)
    {
        Type stateClass = _stateMachineHelper.GetStateClassTypeWithEnum(type);
        object[] args = { _stateMachine };
        EnemyStateBase instance = (EnemyStateBase)Activator.CreateInstance(stateClass, args);

        return instance;
    }

    public EnemyStateBase GetState(EnemyStateType type)
    {
        if (_stateDic.TryGetValue(type, out EnemyStateBase state))
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
