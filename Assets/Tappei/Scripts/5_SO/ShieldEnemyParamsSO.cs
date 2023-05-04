using UnityEngine;

/// <summary>
/// �������̓G�̊e�p�����[�^��ݒ肷��ScriptableObject
/// EnemyParamsManager�Ɏ������A�G���ɎQ�Ƃ���
/// </summary>
[CreateAssetMenu(fileName = "ShieldEnemyParams_")]
public class ShieldEnemyParamsSO : EnemyParamsSO
{
    [Header("�U�����ꂽ�ꍇ�̍d������(�b)�̐ݒ�")]
    [SerializeField] private float _stiffeningTime = 0.5f;

    public float StiffeningTime => _stiffeningTime;
}
