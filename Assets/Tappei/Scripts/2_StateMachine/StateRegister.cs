using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���̃N���X��p���Ďg�p����X�e�[�g�̐����Ɠo�^���s��
/// </summary>
public class StateRegister
{
    /// <summary>
    /// StateType��1:1�őΉ������X�e�[�g����������̂�
    /// �X�e�[�g�̐����������e�ʂ��m�ۂ��Ă���
    /// </summary>
    private static readonly int StateDicCap = Enum.GetValues(typeof(StateType)).Length;

    private Dictionary<StateType, StateTypeBase> _stateDic = new(StateDicCap);

    /// <summary>
    /// ��肤��X�e�[�g�̎�ނ��w�肵�Đ����������^�ɓo�^����
    /// �o�^�����X�e�[�g��GetState()�ɂ���Ď擾�\
    /// </summary>
    public void Register(StateType type, object stateArg)
    {
        if (_stateDic.ContainsKey(type))
        {
            Debug.LogWarning("�����ɃX�e�[�g�����ɓo�^����Ă��܂�: " + type);
            return;
        }

        StateTypeBase state = CreateInstance(type, stateArg);
        _stateDic.Add(type, state);
    }

    private StateTypeBase CreateInstance(StateType type, object stateArg)
    {
        Type stateClass = GetStateClassType(type);

        if (stateClass == null)
        {
            Debug.LogError("StateType�ɃX�e�[�g���R�Â����Ă��܂���: " + type);;
            return null;
        }

        // �X�e�[�g�̃R���X�g���N�^�̈����̏��ɕ��ׂĂ���
        object[] args = { stateArg, type };
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

    /// <summary>
    /// �񋓌^�ɑΉ������X�e�[�g�̃N���X�̌^��Ԃ��̂�
    /// �V�����X�e�[�g��������ۂɂ́A���̏����̕���ɒǉ����ė񋓌^�ƃN���X��R�Â���K�v������
    /// </summary>
    private Type GetStateClassType(StateType type)
    {
        switch (type)
        {
            case StateType.Idle: return typeof(StateTypeIdle);
            case StateType.Search: return typeof(StateTypeSearch);
            case StateType.Attack: return typeof(StateTypeAttack);
            case StateType.Defeated: return typeof(StateTypeDefeated);
            case StateType.Move: return typeof(StateTypeMove);
            default:
                Debug.LogError("�Ή�����X�e�[�g���R�Â����Ă��܂���: " + type);
                return null;
        }
    }
}
