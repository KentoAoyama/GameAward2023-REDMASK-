using UniRx;

/// <summary>
/// �e�X�e�[�g�͂��̃N���X��p���čs���̏������Ăяo�����b�Z�[�W�̑��M���s��
/// ���b�Z�[�W�̎�M�͊e�@�\���s��
/// </summary>
public class BehaviorMessenger
{
    private int _instanceID;

    public BehaviorMessenger(int instanceID)
    {
        _instanceID = instanceID;
    }

    /// <summary>
    /// �e�X�e�[�g�͂��̃��\�b�h���ĂԂ��ƂŊe�@�\�����s���郁�b�Z�[�W�𑗐M����
    /// ���b�Z�[�W�̎�M�͊e�@�\���s��
    /// </summary>
    public void SendMessage(BehaviorType type)
    {
        MessageBroker.Default.Publish(new BehaviorMessage(type, _instanceID));
    }
}
