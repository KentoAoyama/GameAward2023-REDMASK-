using System;
using UnityEngine;

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
            case EnemyStateType.Idle: return typeof(StateTypeIdle);
            case EnemyStateType.Search: return typeof(StateTypeSearch);
            default:
                Debug.LogError("�Ή�����X�e�[�g���R�Â����Ă��܂���: " + type);
                return null;
        }
    }
}
