using UnityEngine;

/// <summary>
/// �������̓G�̊e�p�����[�^��ݒ肷��ScriptableObject
/// EnemyParamsManager�Ɏ������A�G���ɎQ�Ƃ���
/// </summary>
[CreateAssetMenu(fileName = "ShieldEnemyParams_")]
public class ShieldEnemyParamsSO : EnemyParamsSO
{
    public override StateType EntryState
    {
        get
        {
            // �t���O�������Ă���ꍇ��Idle��Ԃɂ����A���Search��ԂƂȂ�
            if (_isAlwaysSearching) return StateType.SearchExtend;

            if (_entryState == State.Idle)
            {
                return StateType.IdleExtend;
            }
            else
            {
                return StateType.SearchExtend;
            }
        }
    }

    [Header("�U�����ꂽ�ꍇ�̍d������(�b)�̐ݒ�")]
    [SerializeField] private float _stiffeningTime = 0.5f;

    public float StiffeningTime => _stiffeningTime;
}
