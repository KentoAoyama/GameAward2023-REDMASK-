using UniRx;
using UniRx.Triggers;
using UnityEngine;

/// <summary>
/// �������p��Reflection��ԂɑΉ�������
/// �e�U�镑���̃N���X�̃��\�b�h��g�ݍ��킹�čs���𐧌䂷��N���X
/// </summary>
public class ShieldEnemyController : EnemyController
{
    [Header("���̃R���C�_�[���t�����I�u�W�F�N�g")]
    [SerializeField] GameObject _shield;

    /// <summary>
    /// ���݂̏�Ԃ���Reflection��ԂɑJ�ڂ��鎖�����肵�����Ɋe�X�e�[�g�ɂ���čX�V�����
    /// Reflection��Ԃ���߂��Ă���ۂɒ��O�̏�Ԃ����Ȃ̂��̏�񂪕K�v
    /// </summary>
    public StateType LastStateType { get; set; }
    public bool IsReflect { get; private set; }
    public ShieldEnemyParamsSO ShieldParams => _enemyParamsSO as ShieldEnemyParamsSO;

    protected override void Awake()
    {
        InitSubscribeShield();
        InitSubscribeReflectionState();
        base.Awake();
    }

    private void InitSubscribeShield()
    {
        _shield.OnDisableAsObservable().Subscribe(_ => IsReflect = true);
    }

    private void InitSubscribeReflectionState()
    {
        _currentState.Skip(1).Where(state => state.Type == StateType.Reflection)
            .Subscribe(_ => IsReflect = false).AddTo(this);
    }

    protected override void InitStateRegister()
    {
        _stateRegister.Register(StateType.IdleExtend, this);
        _stateRegister.Register(StateType.SearchExtend, this);
        _stateRegister.Register(StateType.DiscoverExtend, this);
        _stateRegister.Register(StateType.MoveExtend, this);
        _stateRegister.Register(StateType.AttackExtend, this);
        _stateRegister.Register(StateType.Defeated, this);
        _stateRegister.Register(StateType.Reflection, this);
    }
}
