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

    protected override void Awake()
    {
        InitSubscribeShield();
        base.Awake();
    }

    private void InitSubscribeShield()
    {
        _shield.OnDisableAsObservable().Subscribe(_ => Debug.Log("ヒット"));
    }
}
