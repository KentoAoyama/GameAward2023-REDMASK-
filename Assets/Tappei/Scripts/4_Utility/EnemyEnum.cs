/// <summary>
/// �X�e�[�g�̎�ނ̔�����s���ۂɎg�p����񋓌^
/// �e�X�e�[�g�͕K�����̗񋓌^�̂����̂ǂꂩ�ɑΉ����Ă��Ȃ���΂Ȃ�Ȃ�
/// </summary>
public enum StateType
{
    Unknown,
    Idle,
    Search,
    Discover,
    Move,
    Attack,
    Defeated,
    Reflection,
}

/// <summary>
/// ���E�ɑ΂��ăv���C���[���ǂ̈ʒu�ɂ��邩�̔���Ɏg�p�����񋓌^
/// ���E�̏����Ƃ��̌��ʂ��󂯂Ă̕���Ɏg�p�����
/// </summary>
public enum SightResult
{
    OutSight,
    InSight,
    InAttackRange,
}