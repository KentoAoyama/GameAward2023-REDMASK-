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
    [SerializeField] Sield _shield;

    /// <summary>
    /// ���݂̏�Ԃ���Reflection��ԂɑJ�ڂ��鎖�����肵�����Ɋe�X�e�[�g�ɂ���čX�V�����
    /// Reflection��Ԃ���߂��Ă���ۂɒ��O�̏�Ԃ����Ȃ̂��̏�񂪕K�v
    /// </summary>
    public StateType LastStateType { get; set; }
    public bool IsReflect { get; private set; }
    public ShieldEnemyParamsSO ShieldParams => _enemyParamsSO as ShieldEnemyParamsSO;

    protected override void Awake()
    {
        // �e��K�v�ȃR���|�[�l���g�̎擾���X�e�[�g�}�V���̐ݒ���s��
        base.Awake();
        InitSubscribeShield();
        InitSubscribeReflectionState();
    }

    /// <summary>
    /// ���󂱂̃t���O�����������ǂ�����Reflection�ɑJ�ڂ��Ă���̂�
    /// �������ł��̃t���O�𗧂Ă�Α��v�H
    /// </summary>
    private void InitSubscribeShield()
    {
        // �Ԃɍ��킹�A������Ə����̓o�^����������悤�ɕύX���邱��
        _shield.OnDamaged += () => IsReflect = true;
    }

    /// <summary>
    /// Reflection�ɑJ�ڂ����ꍇ�A�t���O���܂��̂ł�����x�t���O�����Ă�Reflection�ɑJ�ډ\
    /// </summary>
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

    public void MoveForward()
    {
        _moveBehavior.StartMoveForward(Params.AttackRange, Params.RunSpeed);
    }

    /// <summary>
    /// Reflection��Ԃň�莞�Ԃ������炱�̃��\�b�h���ĂԂ��Ƃł�����x���e���ꂪ�o����
    /// </summary>
    public void RecoverShield()
    {
        IsReflect = false;
        _shield.Recover();
    }
}
