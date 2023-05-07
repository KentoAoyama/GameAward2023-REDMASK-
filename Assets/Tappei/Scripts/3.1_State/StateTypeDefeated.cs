/// <summary>
/// ���j���ꂽ��Ԃ̃N���X
/// ����ȏ�͑J�ڂ��Ȃ�
/// </summary>
public class StateTypeDefeated : StateTypeBase
{
    public StateTypeDefeated(EnemyController controller, StateType stateType)
        : base(controller, stateType) { }

    protected override void Enter()
    {
        Controller.PlayAnimation(AnimationName.Dead);
        GameManager.Instance.AudioManager.PlaySE("CueSheet_Gun", "SE_Enemy_Damage");
    }
}