using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �G���g�p���鉹���Ǘ�����N���X
/// EnemyController�o�R�Ŏg�p�����
/// </summary>
public class EnemyAudioModule
{
    // SE�̍Đ�
    // ���j���ꂽ�Ƃ��ɍĐ����̉��͑S���~�߂�
    // �}�b�v�O�ŉ����Ȃ�Ȃ��悤�ɂ�����
    // ����ɂ�����炷�������K�v

    // EnemyController�Ɏ�������̂�State��EnemyController������̂݌Ăяo����
    // �eBehavior����Ăяo���Ȃ������v

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void PlaySE(string name)
    {
        GameManager.Instance.AudioManager.PlaySE("CueSheet_Gun", name);
    }

    public void StopSE()
    {

    }

    public void StopAll()
    {

    }
}
