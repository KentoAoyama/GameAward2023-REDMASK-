using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

/// <summary>
/// ���E�̐���
/// </summary>
public class SightBehavior : MonoBehaviour
{
    [SerializeField] SightSensor _sightSensor;

    ReactiveProperty<bool> _isDetected = new();

    void Awake()
    {
        // ���t���[���Ă΂�郁�\�b�h
        // true���Ԃ����甭��/false�Ŗ�����
        // true���Ԃ�����PlayerFind/false���Ԃ�����PlayerHide
        // 

        // ���t���[�����E�̋@�\���Ă�ł�����肠�Ղ�Ō��m�A�߂��Ԃ�Ń��b�Z�[�W���O����
    }
}
