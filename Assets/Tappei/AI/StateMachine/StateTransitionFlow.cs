using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �e�X�e�[�g�̑J�ڂ�o�^����N���X
/// �X�e�[�g�}�V���͂��̃N���X��p���Ăǂ̃X�e�[�g�ɑJ�ڂ���Ηǂ��̂���m��
/// </summary>
public class StateTransitionFlow : MonoBehaviour
{
    /// <summary>
    /// �C���X�y�N�^�[��Ŋe�X�e�[�g�̑J�ڂ����蓖�Ă邽�߂̍\����
    /// </summary>
    [Serializable]
    private struct TransitionFlow
    {
        [SerializeField] private StateType _currentState;
        [SerializeField] private StateType _nextstate;
        [SerializeField] private StateTransitionTrigger _trigger;

        public StateType CurrentState => _currentState;
        public StateType Nextstate => _nextstate;
        public StateTransitionTrigger Trigger => _trigger;
    }

    [Header("�J�ڌ�/�J�ڐ�/�J�ڏ���")]
    [SerializeField] private TransitionFlow[] _transitionFlow;

    private Dictionary<(StateType, StateTransitionTrigger), StateType> _transitionDic;

    private void Awake()
    {
        InitCreateDic();
    }

    private void InitCreateDic()
    {
        _transitionDic = new(_transitionFlow.Length);
        foreach (TransitionFlow flow in _transitionFlow)
        {
            _transitionDic.Add((flow.CurrentState, flow.Trigger), flow.Nextstate);
        }
    }

    /// <summary>
    /// �X�e�[�g�ƑJ�ڏ����ɑΉ������J�ڐ��StateType���擾����
    /// StateType�ł����ăX�e�[�g���擾���Ă���킯�ł͂Ȃ��̂Œ���
    /// </summary>
    public StateType GetNextStateType(StateType current, StateTransitionTrigger trigger)
    {
        if (_transitionDic.TryGetValue((current, trigger), out StateType next))
        {
            return next;
        }
        else
        {
            Debug.LogError("�J�ڐ悪�o�^����Ă��܂���: " + current + " " + trigger);
            return StateType.Idle;
        }
    }
}
