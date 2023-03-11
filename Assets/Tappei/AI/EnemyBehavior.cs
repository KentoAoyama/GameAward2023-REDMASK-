using UnityEngine;

/// <summary>
/// 敵の各機能の仲介を行うクラス
/// </summary>
public class EnemyBehavior : MonoBehaviour
{
    [SerializeField] EnemyTransitionTimer _enemyTransitionTimer;

    private StateTransitionMessageSender _stateTransitionMessageSender;

    private void Awake()
    {
        _stateTransitionMessageSender = new StateTransitionMessageSender(gameObject.GetInstanceID());
    }

    private void Start()
    {
        _enemyTransitionTimer.DelayedSendTransitionMessage(_stateTransitionMessageSender);
    }

    void Update()
    {

    }
}
