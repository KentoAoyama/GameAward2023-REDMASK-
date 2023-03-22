using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �G�̏e
/// </summary>
public class EnemyRifle : MonoBehaviour, IEnemyWeapon
{
    [Header("���˂���e�̐ݒ�")]
    [SerializeField] private EnemyBullet _enemyBullet;
    [Tooltip("�v�[������G�e�̐��A�U���p�x���グ��ꍇ�͂�������グ�Ȃ��Ƃ����Ȃ�")]
    [SerializeField] private int _poolQuantity;
    [Header("�G�e�𔭎˂���}�Y��")]
    [Tooltip("�e�̓}�Y���̈ʒu����}�Y���̑O�����ɔ��ł���")]
    [SerializeField] private Transform _muzzle;

    private Stack<EnemyBullet> _pool;

    private void Awake()
    {
        _pool = new Stack<EnemyBullet>(_poolQuantity);
        CreatePool();
    }

    private void CreatePool()
    {
        for (int i = 0; i < _poolQuantity; i++)
        {
            EnemyBullet bullet = Instantiate(_enemyBullet, transform.position, Quaternion.identity);
            bullet.gameObject.SetActive(false);
            _pool.Push(bullet);
        }
    }

    /// <summary>
    /// �v�[��������o������
    /// �߂������͒e���Ɏ������Ă���
    /// </summary>
    private EnemyBullet PopPool()
    {
        if (_pool.Count > 0)
        {
            EnemyBullet bullet = _pool.Pop();
            bullet.gameObject.SetActive(true);
            return bullet;
        }
        else
        {
            return null;
        }
    }

    public void Attack()
    {
        EnemyBullet bullet = PopPool();
        bullet.transform.position = _muzzle.position;
        bullet.Shot(_muzzle.forward);
    }
}