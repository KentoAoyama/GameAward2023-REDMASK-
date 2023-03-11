using System;
using UniRx;
using UnityEngine;

/// <summary>
/// StateTransitionMessageSender���瑗�M���ꂽ���b�Z�[�W����M����
/// </summary>
public class StateTransitionMessageReceiver : MonoBehaviour
{
    [Serializable]
    private struct TransitionFlow
    {
        [SerializeField] private EnemyStateType _currentState;
        [SerializeField] private EnemyStateType _nextstate;
        [SerializeField] private StateTransitionTrigger _message;

        public EnemyStateType CurrentState => _currentState;
        public EnemyStateType Nextstate => _nextstate;
        public StateTransitionTrigger Message => _message;
    }

    [Header("�J�ڌ�/�J�ڐ�/�J�ڏ���")]
    [SerializeField] TransitionFlow[] _transitionFlow;

    private int _instanceID;

    private void Awake()
    {
        _instanceID = gameObject.GetInstanceID();

        // �����C���X�^���X���瑗�M���ꂽ���b�Z�[�W�݂̂���M������
        MessageBroker.Default.Receive<StateTransitionMessage>()
            .Where(message => message.ID == _instanceID)
            .Subscribe(message => Debuga(message)).AddTo(this);
    }

    public void Execute(EnemyStateType currentState)
    {
        // ���ł������b�Z�[�W�͑J�ڂ̂��߂̃g���K�[
        // ���̃��\�b�h�͖��t���[���Ă΂��
        

        // ���݂̃X�e�[�g���n�����
        // ����t���[���ɕ����̃��b�Z�[�W�������Ă���ꍇ������̂őΏ�����K�v������H
        // ���b�Z�[�W�����ł��Ă��Ȃ��ꍇ�͂��̃X�e�[�g��Ԃ�
        // ���b�Z�[�W�����ŗ�����n���ꂽ�X�e�[�g�ƑJ�ڏ����Ŏ����Ɍ�����������
    }

    private void Debuga(StateTransitionMessage message)
    {
        Debug.Log(message.Trigger + "����M: "+message.ID);
    }
}
