using UniRx;
using UniRx.Triggers;
using UnityEngine;

/// <summary>
/// 盾持ち用にReflection状態に対応させた
/// 各振る舞いのクラスのメソッドを組み合わせて行動を制御するクラス
/// </summary>
public class ShieldEnemyController : EnemyController
{
    [Header("盾のコライダーが付いたオブジェクト")]
    [SerializeField] GameObject _shield;

    /// <summary>
    /// 現在の状態からReflection状態に遷移する事が決定した時に各ステートによって更新される
    /// Reflection状態から戻ってくる際に直前の状態が何なのかの情報が必要
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

    /// <summary>
    /// 盾が非表示になった場合、フラグが立ってReflectionに遷移する
    /// 現状このフラグが立ったかどうかでReflectionに遷移しているので
    /// 一定周期でこのフラグを立てれば大丈夫？
    /// </summary>
    private void InitSubscribeShield()
    {
        _shield.OnDisableAsObservable().Subscribe(_ => IsReflect = true);
    }

    /// <summary>
    /// Reflectionに遷移した場合、フラグが折れるのでもう一度フラグが立てばReflectionに遷移可能
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
}
