using UniRx;

/// <summary>
/// �e�X�e�[�g�͂��̃N���X��p���čs���̏������Ăяo�����b�Z�[�W�̑��M���s��
/// ���b�Z�[�W�̎�M��EnemyActor�N���X���s��
/// </summary>
public class BehaviorMessenger
{
    private int _instanceID;

    public BehaviorMessenger(int instanceID)
    {
        _instanceID = instanceID;
    }

    /// <summary>
    /// �e�X�e�[�g�͂��̃��\�b�h�Ń��b�Z�[�W�𑗐M���邱�ƂŊe�s���̏������Ăяo��
    /// ���b�Z�[�W�̎�M��EnemyActor�N���X���s��
    /// </summary>
    public void SendMessage(BehaviorType type)
    {
        MessageBroker.Default.Publish(new BehaviorMessage(type, _instanceID));
    }
}
