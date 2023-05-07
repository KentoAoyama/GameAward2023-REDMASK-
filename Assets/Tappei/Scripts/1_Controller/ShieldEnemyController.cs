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
    /// <summary>
    /// �������̓G�p�̃v���p�e�B���Q�Ƃ���ꍇ�ɃL���X�g���Ďg���K�v������
    /// </summary>
    public ShieldEnemyParamsSO ShieldParams => _enemyParamsSO as ShieldEnemyParamsSO;
    public bool IsReflect { get; private set; }

    protected override void InitOnAwake()
    {
        _stateRegister.Register(StateType.IdleExtend, this);
        _stateRegister.Register(StateType.SearchExtend, this);
        _stateRegister.Register(StateType.DiscoverExtend, this);
        _stateRegister.Register(StateType.MoveExtend, this);
        _stateRegister.Register(StateType.AttackExtend, this);
        _stateRegister.Register(StateType.Defeated, this);
        _stateRegister.Register(StateType.Reflection, this);
        _currentState.Value = _stateRegister.GetState(StateType.IdleExtend);

        // ���ɒe���q�b�g������Reflection��ԂɑJ�ڂ���t���O�𗧂Ă�
        _shield.OnDamaged += () => IsReflect = true;
        this.OnDisableAsObservable().Subscribe(_ => _shield.OnDamaged -= () => IsReflect = true);
    }

    /// <summary>
    /// �U������ۂɑO���Ɉړ�����̂ŁA���̃��\�b�h���Ă�ň�莞�Ԍo�ߌ�ɍU���̏������Ă�
    /// </summary>
    public void MoveForward() => _moveBehavior.StartMoveForward(Params.AttackRange, Params.RunSpeed);

    /// <summary>
    /// Reflection��Ԃň�莞�Ԃ������炱�̃��\�b�h���ĂԂ��Ƃł�����x���e���ꂪ�o����
    /// </summary>
    public void RecoverShield()
    {
        IsReflect = false;
        _shield.Recover();
    }
}
