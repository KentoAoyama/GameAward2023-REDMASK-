using System;
using UnityEngine;

/// <summary>
/// �G�̃X�e�[�g�}�V���Ŏ�肤��X�e�[�g�̎��
/// �e�X�e�[�g�͕K�����̗񋓌^�̂����̂ǂꂩ�ɑΉ����Ă��Ȃ���΂Ȃ�Ȃ�
/// </summary>
public enum EnemyStateType
{
    Base, // �e�X�e�[�g�̊��N���X�p
    Idle,
    Search,
    Move,
    Attack,
    Damage,
    Death,
    Reflection,
}

/// <summary>
/// �ėp�I�ȏ������܂Ƃ߂��N���X
/// </summary>
public class EnemyStateMachineHelper
{
    /// <summary>
    /// �񋓌^�ɑΉ������X�e�[�g�̃N���X�̌^��Ԃ��̂�
    /// �V�����X�e�[�g��������ۂɂ́A���̏����̕���ɒǉ����ė񋓌^�ƃN���X��R�Â���K�v������
    /// </summary>
    public Type GetStateClassTypeWithEnum(EnemyStateType type)
    {
        switch (type)
        {
            case EnemyStateType.Idle: return typeof(EnemyStateIdle);
            case EnemyStateType.Search: return typeof(EnemyStateSearch);
            default:
                Debug.LogError("�Ή�����X�e�[�g���R�Â����Ă��܂���: " + type);
                return null;
        }
    }
}
