using UniRx;

/// <summary>
/// �X�e�[�g�J�ڂ̃��b�Z�[�W�̑��M���s��
/// ���b�Z�[�W�̎�M�ɂ�StateTransitionMessageReceiver�N���X�ōs��
/// </summary>
public class StateTransitionMessageSender
{
    public StateTransitionMessageSender(int instanceID)
    {
        InstanceID = instanceID;
    }

    public int InstanceID { get; }

    /// <summary>
    /// �e�@�\�̃N���X�͂��̃��\�b�h���ĂԂ��ƂŃX�e�[�g�}�V����
    /// �J�ڂ̏����𖞂��������Ƃ�`����
    /// </summary>
    public void SendMessage(StateTransitionTrigger trigger)
    {
        MessageBroker.Default.Publish(new StateTransitionMessage(trigger, InstanceID));
    }
}
