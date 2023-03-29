using UnityEngine;

/// <summary>
/// 盾持ちの敵が装備する盾
/// 弾がヒットしたときに非表示になったのをShieldEnemyController側が検知する
/// </summary>
public class Sield : MonoBehaviour, IDamageable
{
    public void Damage() => gameObject.SetActive(false);
}
