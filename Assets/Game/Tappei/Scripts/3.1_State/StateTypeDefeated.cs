/// <summary>
/// 通常/盾持ち両方が使用する撃破された状態のクラス
/// これ以上は遷移しない
/// </summary>
public class StateTypeDefeated : StateTypeBase
{
    public StateTypeDefeated(EnemyController controller, StateType stateType)
        : base(controller, stateType) { }

    protected override void Enter()
    {
        Controller.PlayAnimation(AnimationName.Dead);
        if(GameManager.Instance.TimeController.EnemyTime < 1.0f)
        {
            GameManager.Instance.AudioManager.PlaySE("CueSheet_Gun", "SE_Enemy_Damage_Slow");
        }
        else
        {
            GameManager.Instance.AudioManager.PlaySE("CueSheet_Gun", "SE_Enemy_Damage");
        }
    }
}