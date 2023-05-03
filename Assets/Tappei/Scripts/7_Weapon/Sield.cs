using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 盾持ちの敵が装備する盾
/// 一度弾を受けたら任意のタイミングまで無効化される
/// </summary>
public class Sield : MonoBehaviour, IDamageable
{
    private Collider2D _collider;

    /// <summary>
    /// 盾にプレイヤーの弾がヒットしたときの処理
    /// このコールバックが呼ばれる度にRef
    /// </summary>
    public UnityAction OnDamaged;

    private void Awake()
    {
        _collider = GetComponent<BoxCollider2D>();
    }

    public void Damage()
    {
        _collider.enabled = false;
        OnDamaged.Invoke();
    }

    public void Recover() => _collider.enabled = true;
}
