using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

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
        int PlayerBulletLayer = LayerMask.NameToLayer("PlayerBullet");

        _shield.OnTriggerEnter2DAsObservable()
            .Where(c => c.gameObject.layer == PlayerBulletLayer)
            .Subscribe(c => Debug.Log("ヒット"));
    }
}
