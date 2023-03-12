using UnityEngine;

/// <summary>
/// 敵の各機能の仲介を行うクラス
/// </summary>
public class EnemyBehavior : MonoBehaviour
{
    [SerializeField] EnemyTransitionTimer _enemyTransitionTimer;

    private StateTransitionMessenger _stateTransitionMessageSender;

    private void Awake()
    {
        _stateTransitionMessageSender = new StateTransitionMessenger(gameObject.GetInstanceID());
    }

    private void Start()
    {
        _enemyTransitionTimer.DelayedSendTransitionMessage(_stateTransitionMessageSender);
    }

    void Update()
    {

    }
}
