using UniRx;
using UniRx.Triggers;
using UnityEngine;

/// <summary>
/// �������p
/// �e�U�镑���̃N���X�̃��\�b�h��g�ݍ��킹�čs���𐧌䂷��N���X
/// </summary>
public class ShieldEnemyController : EnemyController
{
    [Header("���̃R���C�_�[���t�����I�u�W�F�N�g")]
    [SerializeField] GameObject _shield;

    /// <summary>
    /// Reflection��Ԃ���J�ڂ���ۂɎg�p����
    /// ���݂̏�Ԃ���Reflection��ԂɑJ�ڂ��鎖�����肵�����ɍX�V�����
    /// </summary>
    StateType _lastStateType;

    public bool IsReflect { get; private set; }

    protected override void Awake()
    {
        InitSubscribeShield();
        base.Awake();
    }

    private void InitSubscribeShield()
    {
        _shield.OnDisableAsObservable().Subscribe(_ => IsReflect = true);
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
