using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

/// <summary>
/// �G�̃X�e�[�g�}�V��
/// </summary>
public class EnemyStateMachine : MonoBehaviour, IPausable
{
    private ReactiveProperty<EnemyStateBase> _currentState = new();
    private EnemyStateMachineStateRegister _stateRegister;

    public IReadOnlyReactiveProperty<EnemyStateBase> CurrentState;

    private void Awake()
    {
        EnemyStateMachineHelper helper = new();
        _stateRegister = new(this, helper);
        _stateRegister.Register(EnemyStateType.Idle);
        _stateRegister.Register(EnemyStateType.Search);

        _currentState.Value = _stateRegister.GetState(EnemyStateType.Search);
    }

    void Start()
    {
        // ��ԑJ�ڂɎg�����́A�����J�ڂ݂̂������āA�J�ڂ��邩��Ԃ��N���X��p�ӂ���
        // �v���C���[�𔭌�����
        // �v���C���[����������
        // �_���[�W���󂯂�
        // �̗͂����ȉ��ɂȂ���
        // ��莞�Ԃ�������
    }

    void Update()
    {
        _currentState.Value.Update();
    }



    public void Pause() => _currentState.Value.Pause();
    public void Resume() => _currentState.Value.Resume();
}