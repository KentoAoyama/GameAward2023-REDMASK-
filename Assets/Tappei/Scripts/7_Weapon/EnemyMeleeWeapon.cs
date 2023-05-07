using UnityEngine;

/// <summary>
/// �ߐڍU���̕���̃N���X
/// Enemy_RangeAttack�I�u�W�F�N�g���g�p����
/// </summary>
public class EnemyMeleeWeapon : MonoBehaviour, IEnemyWeapon
{
    [Header("�U���͈�")]
    [SerializeField] private float _radius;
    [Header("�v���C���[�������郌�C���[")]
    [SerializeField] private LayerMask _playerLayerMask;
    [Header("�U�����ɍĐ�����鉹�̖��O")]
    [SerializeField] private string _attackSEName;

    /// <summary>�v���C���[�݂̂����o����̂Œ�����1�ŗǂ�</summary>
    private Collider2D[] _results = new Collider2D[1];

    public void Attack()
    {
        int hitCount = Physics2D.OverlapCircleNonAlloc(transform.position, _radius, _results, _playerLayerMask);
        if (hitCount > 0)
        {
            _results[0].GetComponent<IDamageable>().Damage();
        }

        GameManager.Instance.AudioManager.PlaySE("CueSheet_Gun", _attackSEName);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _radius);
    }
}
