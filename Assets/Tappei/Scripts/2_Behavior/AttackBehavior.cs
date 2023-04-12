using UnityEngine;

/// <summary>
/// �U�����s���ۂɎg�p����N���X
/// </summary>
public class AttackBehavior : MonoBehaviour
{
    [Header("����̃I�u�W�F�N�g")]
    [SerializeField] private MonoBehaviour _weapon;

    IEnemyWeapon _enemyWeapon;

    private void Awake()
    {
        if (!_weapon.TryGetComponent(out _enemyWeapon))
        {
            Debug.LogError("IEnemyWeapon�����������R���|�[�l���g�ł͂���܂���: " + _weapon);
        }
    }

    public void Attack() => _enemyWeapon.Attack();
}
