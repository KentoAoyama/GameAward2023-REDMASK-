using UniRx;
using UniRx.Triggers;
using UnityEngine;

/// <summary>
/// 盾持ち用
/// 各振る舞いのクラスのメソッドを組み合わせて行動を制御するクラス
/// </summary>
public class ShieldEnemyController : EnemyController
{
    [Header("盾のコライダーが付いたオブジェクト")]
    [SerializeField] GameObject _shield;

    /// <summary>
    /// Reflection状態から遷移する際に使用する
    /// 現在の状態からReflection状態に遷移する事が決定した時に更新される
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
        // 登録処理
    }
}
