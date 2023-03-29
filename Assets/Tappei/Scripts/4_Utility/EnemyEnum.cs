/// <summary>
/// �X�e�[�g�̎�ނ̔�����s���ۂɎg�p����񋓌^
/// �e�X�e�[�g�͕K�����̗񋓌^�̂����̂ǂꂩ�ɑΉ����Ă��Ȃ���΂Ȃ�Ȃ�
/// </summary>
public enum StateType
{
    Unknown,
    Idle,
    IdleExtend,
    Search,
    SearchExtend,
    Discover,
    DiscoverExtend,
    Move,
    MoveExtend,
    Attack,
    AttackExtend,
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

/// <summary>
/// �A�j���[�V�������̎擾�Ɏg�p�����񋓌^
/// �񋓌^����n�b�V�����擾����̂Ɏg�p����̂Œl�͊eAnimation���ƈ�v�����邱��
/// </summary>
public enum AnimationName
{
    Idle,
    Search,
    Discover,
    Move,
    Attack,
    Death,
    Reflection,
}