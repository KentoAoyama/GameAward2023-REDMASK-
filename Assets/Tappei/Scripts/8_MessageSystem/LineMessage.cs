/// <summary>
/// �\�������䎌�̍\����
/// ��������b�Z�[�W���O�ł���肷��
/// </summary>
public readonly struct LineMessage
{
    public LineMessage(string line)
    {
        Line = line;
    }

    public string Line { get; }
}
