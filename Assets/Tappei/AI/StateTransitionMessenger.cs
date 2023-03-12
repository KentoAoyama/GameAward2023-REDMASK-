using UniRx;

/// <summary>
/// �X�e�[�g�J�ڂ̃��b�Z�[�W�̑��M���s���N���X
/// ���b�Z�[�W�̎�M�̓X�e�[�g�}�V�����s��
/// </summary>
public class StateTransitionMessenger
{
    private int _instanceID;

    public StateTransitionMessenger(int instanceID)
    {
        _instanceID = instanceID;
    }

    /// <summary>
    /// �e�@�\�̃N���X�͂��̃��\�b�h���ĂԂ��ƂŃX�e�[�g�}�V����
    /// �J�ڂ̏����𖞂������Ƃ������b�Z�[�W�𑗐M����
    /// </summary>
    public void SendMessage(StateTransitionTrigger trigger)
    {
        MessageBroker.Default.Publish(new StateTransitionMessage(trigger, _instanceID));
    }
}
