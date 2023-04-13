using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drum : MonoBehaviour, IDamageable
{

    [Header("�I�u�W�F�N�g�������܂ł̎���")]
    [Tooltip("�I�u�W�F�N�g�������܂ł̎���"), SerializeField]
    private float _destroyTime;

    [Header("�_���[�W���Ăяo���郌�C���[")]
    [Tooltip("�_���[�W���Ăяo���郌�C���["), SerializeField]
    private LayerMask _layer;

    [Header("�����͈͂̎����A���^or�~�`")]
    [Tooltip("�����͈͂̎����A���^or�~�`"), SerializeField]
    private ShapeType _checkType = ShapeType.Box;

    [Header("���^�̔���̒��S")]
    [Tooltip("���^�̔���̒��S"), SerializeField]
    private Vector2 _boxCenterOffSet;

    [Header("���^�̔���̑傫��")]
    [Tooltip("���^�̔���̑傫��"), SerializeField]
    private Vector2 _boxSize;

    [Header("�~�`�̔���̒��S")]
    [Tooltip("�~�`�̔���̒��S"), SerializeField]
    private Vector2 _circleCenterOffSet;

    [Header("�~�`�̔���̑傫��")]
    [Tooltip("�~�`�̔���̑傫��"), SerializeField]
    private float _circleRadius;

    [Header("�h�����ʂ̉摜�p�̎q�I�u�W�F�N�g")]
    [Tooltip("�h�����ʂ̉摜�p�̎q�I�u�W�F�N�g"), SerializeField]
    private GameObject _image;

    [Header("�����̃G�t�F�N�g")]
    [Tooltip("�����̃G�t�F�N�g"), SerializeField]
    private ParticleSystem _particleSystem;

    private float _countDestriyTime;

    /// <summary>�����������ǂ���</summary>
    private bool _isExprosion = false;

    private enum ShapeType
    {
        //�~�`�̔���
        Circle,
        //���^�̔���
        Box
    }

    void Update()
    {
        CountDestroyTime();
    }

    private void OnDrawGizmosSelected()
    {
        if (_checkType == Drum.ShapeType.Box)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube((Vector2)transform.position + _boxCenterOffSet, _boxSize);
        }
        else if (_checkType == Drum.ShapeType.Circle)
        {
            Gizmos.color = Color.red;

            Gizmos.DrawWireSphere((Vector2)transform.position + _circleCenterOffSet, _circleRadius);
        }
    }

    /// <summary>�͈͓���IDamageble�̎擾/���s�����݂�</summary>
    private void CallDamage()
    {
        //���^�Ŕ��肷��ꍇ
        if (_checkType == Drum.ShapeType.Box)
        {
            //�͈͓����`�F�b�N����
            Collider2D[] hits = Physics2D.OverlapBoxAll((Vector2)transform.position + _boxCenterOffSet, _boxSize, 0, _layer);

            //�擾�����R���C�_�[�ɑ΂��āAIDamageble�̎擾/���s�����݂�
            foreach (var hit in hits)
            {
                //IDamageble�̎擾�����݂�
                hit.TryGetComponent<IDamageable>(out IDamageable damageable);

                //IDamageble�̎��s�����݂�
                damageable?.Damage();
            }

        }   //�~�`�Ŕ��肷��ꍇ
        else if (_checkType == Drum.ShapeType.Circle)
        {
            //�͈͓����`�F�b�N����
            Collider2D[] hits = Physics2D.OverlapCircleAll((Vector2)transform.position + _circleCenterOffSet, _circleRadius, _layer);

            //�擾�����R���C�_�[�ɑ΂��āAIDamageble�̎擾/���s�����݂�
            foreach (var hit in hits)
            {
                //IDamageble�̎擾�����݂�
                hit.TryGetComponent<IDamageable>(out IDamageable damageable);

                //IDamageble�̎��s�����݂�
                damageable?.Damage();
            }
        }

    }

    /// <summary>�C���^�[�t�F�C�X�̊֐��B�����J�n����</summary>
    public void Damage()
    {
        //���ɔ������Ă�����Ă΂Ȃ��B2�x�Ă΂�Ȃ��悤�ɂ��Ă���B
        if (_isExprosion) return;

        //�͈͓��ɂ��镨�ɑ΂��ă_���[�W���������s
        CallDamage();

        //�h�����ʂ̉摜������
        _image.SetActive(false);

        //�����̃G�t�F�N�g���Đ�
        _particleSystem.Play();

        _isExprosion = true;

        Debug.Log("D");
    }

    /// <summary>�������Ă���I�u�W�F�N�g�������܂ł̎��Ԃ��v������֐�</summary>
    public void CountDestroyTime()
    {
        if (!_isExprosion) return;

        //���Ԃ��v������
        _countDestriyTime += Time.deltaTime * GameManager.Instance.TimeController.EnemyTime;

        //�����܂ł̎��Ԃ������������
        if (_countDestriyTime > _destroyTime)
        {
            Destroy(gameObject);
        }
    }
}
