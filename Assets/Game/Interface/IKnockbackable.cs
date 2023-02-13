using UnityEngine;

/// <summary>
/// ノックバック可能を表すインターフェース
/// </summary>
public interface IKnockbackable
{
    public void Knockback(Vector2 dir, float power);
}