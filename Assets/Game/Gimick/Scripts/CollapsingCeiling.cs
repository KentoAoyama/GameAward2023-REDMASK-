using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollapsingCeiling : MonoBehaviour,IDamageable
{
    [Header("�I�u�W�F�N�g�������܂ł̎���")]
    [Tooltip("�I�u�W�F�N�g�������܂ł̎���"), SerializeField]
    private float _destroyTime;

    [Header("�_���[�W���Ăяo���郌�C���[")]
    [Tooltip("�_���[�W���Ăяo���郌�C���["), SerializeField]
    private LayerMask _layer;



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

    [Header("�V��̉摜�p�̎q�I�u�W�F�N�g")]
    [Tooltip("�V��̉摜�p�̎q�I�u�W�F�N�g"), SerializeField]
    private GameObject _image;

    [Header("���I�̉摜�p�̎q�I�u�W�F�N�g")]
    [Tooltip("���I�̉摜�p�̎q�I�u�W�F�N�g"), SerializeField]
    private GameObject _rubbleImage;


    [Header("���I�̃G�t�F�N�g")]
    [Tooltip("���I�̃G�t�F�N�g"), SerializeField]
    private ParticleSystem _particleSystem;

    [Header("���̃G�t�F�N�g")]
    [Tooltip("���̃G�t�F�N�g"), SerializeField]
    private ParticleSystem _smokeParticleSystem;

    private float _countDestriyTime;

    /// <summary>�����������ǂ���</summary>
    private bool _isBrake = false;

    void Update()
    {
        CountDestroyTime();
    }

    private void OnDrawGizmosSelected()
    {

    }


    /// <summary>���I���n�ʂɗ��������ǂ������m�F����</summary>
    private void CheckDownRubble()
    {


    }


    /// <summary>�C���^�[�t�F�C�X�̊֐��B�����J�n����</summary>
    public void Damage()
    {
        //���ɔ������Ă�����Ă΂Ȃ��B2�x�Ă΂�Ȃ��悤�ɂ��Ă���B
        if (_isBrake) return;

        //�V��̉摜������
        _image.SetActive(false);

        //�����̃G�t�F�N�g���Đ�
        _particleSystem.Play();

        _isBrake = true;

        Debug.Log("D");
    }

    /// <summary>�������Ă���I�u�W�F�N�g�������܂ł̎��Ԃ��v������֐�</summary>
    public void CountDestroyTime()
    {
        if (!_isBrake) return;

        //���Ԃ��v������
        _countDestriyTime += Time.deltaTime * GameManager.Instance.TimeController.EnemyTime;

        //�����܂ł̎��Ԃ������������
        if (_countDestriyTime > _destroyTime)
        {
            //�����o��
            _smokeParticleSystem.Play();
            //���I���摜���o��
            _rubbleImage.SetActive(true);

            //���I�̃p�[�e�B�N��������
            Destroy(_particleSystem);
        }
    }
}
