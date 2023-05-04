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
    /// 盾にプレイヤーの弾がヒットしたときのコールバック
    /// </summary>
    public UnityAction OnDamaged;

    private void Awake()
    {
        _collider = GetComponent<BoxCollider2D>();
    }

    public void Damage()
    {
        _collider.enabled = false;
        OnDamaged?.Invoke();
    }

    /// <summary>
    /// 外部からこのメソッドを呼ぶことで盾が有効化される
    /// </summary>
    public void Recover() => _collider.enabled = true;
}
