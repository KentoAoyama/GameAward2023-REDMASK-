/// <summary>
/// �G�̃X�e�[�g�}�V���Ŏ�肤��X�e�[�g�̎��
/// �e�X�e�[�g�͕K�����̗񋓌^�̂����̂ǂꂩ�ɑΉ����Ă��Ȃ���΂Ȃ�Ȃ�
/// </summary>
public enum StateType
{
    Base, // �e�X�e�[�g�̊��N���X�p
    Idle,
    Search,
    Move,
    Attack,
    Damage,
    Death,
    Reflection,
}

/// <summary>
/// �X�e�[�g�J�ڂ̃g���K�[�ƂȂ�C�x���g�̗񋓌^
/// ���̗񋓌^���܂ރ��b�Z�[�W�̑���M�Ɏg�p����
/// </summary>
public enum StateTransitionTrigger
{
    TimeElapsed,
    PlayerFind,
    PlayerHide,
    PlayerInAttackRange,
    PlayerOutAttackRange,
}