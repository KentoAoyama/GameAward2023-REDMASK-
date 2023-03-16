using System;
using UnityEngine;

/// <summary>
/// �X�e�[�g�}�V���Ŏg�p����ėp�I�ȏ������܂Ƃ߂��w���p�[�N���X
/// </summary>
public class StateMachineHelper
{
    /// <summary>
    /// �񋓌^�ɑΉ������X�e�[�g�̃N���X�̌^��Ԃ��̂�
    /// �V�����X�e�[�g��������ۂɂ́A���̏����̕���ɒǉ����ė񋓌^�ƃN���X��R�Â���K�v������
    /// </summary>
    public Type GetStateClassTypeWithEnum(StateType type)
    {
        switch (type)
        {
            case StateType.Idle: return typeof(StateTypeIdle);
            case StateType.Search: return typeof(StateTypeSearch);
            default:
                Debug.LogError("�Ή�����X�e�[�g���R�Â����Ă��܂���: " + type);
                return null;
        }
    }
}