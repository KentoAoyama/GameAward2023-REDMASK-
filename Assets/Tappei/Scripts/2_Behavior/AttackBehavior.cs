using UnityEngine;

/// <summary>
/// 攻撃を行う際に使用するクラス
/// </summary>
public class AttackBehavior : MonoBehaviour
{
    [Header("武器のオブジェクト")]
    [SerializeField] private MonoBehaviour _weapon;

    private IEnemyWeapon _enemyWeapon;

    private void Awake()
    {
        if (!_weapon.TryGetComponent(out _enemyWeapon))
        {
            Debug.LogError("IEnemyWeaponを実装したコンポーネントではありません: " + _weapon);
        }
    }

    public void Attack()
    {
        // TODO:攻撃の判定が出るまでディレイが欲しい

        _enemyWeapon.Attack();
    }
}
