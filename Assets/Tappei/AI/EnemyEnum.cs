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
    Defeated,
    Reflection,
}

/// <summary>
/// �X�e�[�g�J�ڂ̃g���K�[�ƂȂ�C�x���g�̎��
/// ���̗񋓌^���܂ރ��b�Z�[�W���󂯎������X�e�[�g���J�ڂ���
/// </summary>
public enum StateTransitionTrigger
{
    TimeElapsed,
    PlayerFind,
    PlayerHide,
    PlayerInAttackRange,
    PlayerOutAttackRange,
}

/// <summary>
/// ��肤��s���̎��
/// �e�X�e�[�g���Ή������s���̏������Ăяo���̂Ɏg�p����
/// </summary>
public enum BehaviorType
{
    Attack,
    SearchMove,
    StopMove,
    Defeated,
}