using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

/// <summary>
/// �w�肵�������ɂ܂�������ԓG�e�̃N���X
/// EnemyRifle�N���X�Ƀv�[������Ă���A���˂���ۂɃA�N�e�B�u�ɂȂ�
/// </summary>
public class EnemyBullet : MonoBehaviour
{
    [Header("�e�̐ݒ�")]
    [SerializeField] private float _speed;
    [Tooltip("��莞�Ԍ�ɔ�A�N�e�B�u�ɂȂ�v�[���ɖ߂�")]
    [SerializeField] private float _lifeTime;

    private Stack<EnemyBullet> _pool;
    private Tween _tween;
    private Transform _transform;

    private void Awake()
    {
        _transform = transform;
    }

    private void OnDisable()
    {
        _tween?.Kill();
    }

    public void InitSetPool(Stack<EnemyBullet> pool) => _pool = pool;

    /// <summary>
    /// ���˂����ۂ�EnemyRifle�N���X����Ăяo�����
    /// �����̕����ɔ�сA���Ԍo�߂Ńv�[���ɖ߂�
    /// </summary>
    public void Shot(Vector3 dir)
    {
        Vector3 forward = dir.normalized;
        _tween = DOVirtual.DelayedCall(_lifeTime, ReturnPool)
            .OnUpdate(() => _transform.Translate(forward * _speed * GameManager.Instance.TimeController.EnemyTime));
    }

    /// <summary>
    /// �v�[���ɖ߂�
    /// ���˂���Ă����莞�Ԍ�A�������̓v���C���[�Ƀq�b�g�����ۂɌĂ΂��
    /// </summary>
    private void ReturnPool()
    {
        gameObject.SetActive(false);
        _pool?.Push(this);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && 
            collision.gameObject.TryGetComponent(out IDamageable damageable))
        {
            damageable.Damage();
            ReturnPool();
        }
    }
}
