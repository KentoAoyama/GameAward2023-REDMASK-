using UniRx;
using UniRx.Triggers;
using UnityEngine;

/// <summary>
/// 盾持ち用にReflection状態に対応させた
/// 各振る舞いのクラスのメソッドを組み合わせて行動を制御するクラス
/// </summary>
public class ShieldEnemyController : EnemyController
{
    [Tooltip("盾のコライダーが付いたオブジェクト")]
    [SerializeField] Sield _shield;

    /// <summary>
    /// 現在の状態からReflection状態に遷移する事が決定した時に各ステートによって更新される
    /// Reflection状態から戻ってくる際に直前の状態が何なのかの情報が必要
    /// </summary>
    public StateType LastStateType { get; set; }
    /// <summary>
    /// 盾持ちの敵用のプロパティを参照する場合にキャストして使う必要がある
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
        _stateRegister.Register(StateType.ReactionExtend, this);
        _currentState.Value = _stateRegister.GetState(StateType.IdleExtend);

        // 盾に弾がヒットしたらReflection状態に遷移するフラグを立てる
        _shield.OnDamaged += () => IsReflect = true;
        this.OnDisableAsObservable().Subscribe(_ => _shield.OnDamaged -= () => IsReflect = true);
    }

    /// <summary>
    /// 攻撃する際に前方に移動するので、このメソッドを呼んで一定時間経過後に攻撃の処理を呼ぶ
    /// </summary>
    public void MoveForward() => _moveBehavior.StartMoveForward(Params.AttackRange, Params.RunSpeed);

    /// <summary>
    /// Reflection状態で一定時間たったらこのメソッドを呼ぶことでもう一度盾弾かれが出来る
    /// </summary>
    public void RecoverShield()
    {
        IsReflect = false;
        _shield.Recover();
    }
}
