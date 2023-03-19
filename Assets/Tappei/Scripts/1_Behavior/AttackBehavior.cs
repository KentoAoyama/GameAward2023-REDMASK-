using UnityEngine;

/// <summary>
/// �U�����s���ۂɎg�p����N���X
/// </summary>
public class AttackBehavior : MonoBehaviour
{
    [Header("�e�̃v���n�u")]
    [SerializeField] GameObject _bulletPreafb;
    [Header("�e�𔭎˂���ʒu")]
    [SerializeField] Transform _muzzle;

    public void Attack()
    {
        GameObject instance = Instantiate(_bulletPreafb, _muzzle.position, Quaternion.identity);
        instance.GetComponent<EnemyTestBullet>().Init(_muzzle.localScale.x);
    }
}
