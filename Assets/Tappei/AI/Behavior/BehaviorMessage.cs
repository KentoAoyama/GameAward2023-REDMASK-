/// <summary>
/// ���̍\���̂̃��b�Z�[�W�𑗎�M���邱�ƂŊe�@�\���Ăяo��
/// BehaviorMessenger�N���X���瑗�M����A�e�@�\����M����
/// </summary>
public struct BehaviorMessage
{
    public BehaviorMessage(BehaviorType type, int id)
    {
        Type = type;
        ID = id;
    }

    public BehaviorType Type { get; }
    public int ID { get; }
}
