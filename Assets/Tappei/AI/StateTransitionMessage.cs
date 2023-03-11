/// <summary>
/// ���̍\���̂̃��b�Z�[�W�𑗎�M���邱�ƂŃX�e�[�g�̑J�ڂ��s��
/// StateTransitionMessageSender/Receiver�N���X�ő��M/��M������
/// </summary>
public struct StateTransitionMessage
{
    public StateTransitionMessage(StateTransitionTrigger trigger, int id)
    {
        Trigger = trigger;
        ID = id;
    }

    public StateTransitionTrigger Trigger { get; }
    public int ID { get; }
}