using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �������U���̕���̃N���X
/// Enemy_RangeAttack�I�u�W�F�N�g���g�p����
/// </summary>
public class EnemyRifle : MonoBehaviour, IEnemyWeapon
{
    /// <summary>
    /// �G���ɂ��̃I�u�W�F�N�g�̎q�ɒe�𐶐����Ă���
    /// �h���[���Ɖ������U���̓G���g�p����e�������Ƃ����z��
    /// </summary>
    private static Transform _poolObject;

    [Header("���˂���e�Ɋւ���ݒ�")]
    [SerializeField] private EnemyBullet _enemyBullet;
    [Tooltip("�v�[������G�e�̐��A�U���p�x���グ��ꍇ�͂�������グ�Ȃ��Ƃ����Ȃ�")]
    [SerializeField] private int _poolQuantity;
    [Tooltip("�e�����˂����}�Y���A��ԕ����̍��E�̐���̓X�P�[����x��-1�ɂ��邱�Ƃōs��")]
    [SerializeField] protected Transform _muzzle;
    [Header("�U�����ɍĐ�����鉹�̖��O")]
    [SerializeField] private string _attackSEName;

    private Stack<EnemyBullet> _pool;

    protected virtual void Awake()
    {
        if (_poolObject == null)
        {
            CreatePoolObject();
        }

        _pool = new Stack<EnemyBullet>(_poolQuantity);
        CreatePool();
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private void ReleasePoolObjectInstance() => _poolObject = null;

    private void CreatePoolObject()
    {
        GameObject poolObject = new GameObject();
        poolObject.name = "EnemyBulletPool";
        _poolObject = poolObject.transform;
    }

    private void CreatePool()
    {
        for (int i = 0; i < _poolQuantity; i++)
        {
            EnemyBullet bullet = Instantiate(_enemyBullet, transform.position, Quaternion.identity, _poolObject);
            bullet.InitSetPool(_pool);
            bullet.gameObject.SetActive(false);
            _pool.Push(bullet);
        }
    }

    /// <summary>
    /// �v�[��������o������
    /// �߂������͒e���Ɏ������Ă���
    /// </summary>
    protected EnemyBullet PopPool()
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
        if (bullet == null) return;

        bullet.transform.position = _muzzle.position;
        bullet.SetVelocity(GetBulletDirection());

        GameManager.Instance.AudioManager.PlaySE("CueSheet_Gun", _attackSEName);
    }

    protected virtual Vector3 GetBulletDirection() => _muzzle.right * _muzzle.transform.localScale.x;
}