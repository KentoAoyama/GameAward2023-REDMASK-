using UnityEngine;

/// <summary>
/// �U�����s���ۂɎg�p����N���X
/// �e�X�e�[�g����BehaviorFacade�N���X�o�R�ŌĂяo�����
/// </summary>
public class AttackBehavior : MonoBehaviour
{
    [Header("�e�̃v���n�u")]
    [SerializeField] private GameObject _bulletPreafb;
    [Header("�e�𔭎˂���ʒu")]
    [SerializeField] private Transform _muzzle;

    public void Attack()
    {
        Debug.Log("�U��");
    }
}
