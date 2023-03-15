using UnityEngine;

/// <summary>
/// 敵の各機能の仲介を行うクラス
/// </summary>
public class EnemyActor : MonoBehaviour
{
    [SerializeField] TransitionWithTimeElapsed _transitionWithTimeElapsed;

    private StateTransitionMessenger _stateTransitionMessageSender;

    private void Awake()
    {
        _stateTransitionMessageSender = new StateTransitionMessenger(gameObject.GetInstanceID());
    }

    private void Start()
    {
        _transitionWithTimeElapsed.DelayedSendTransitionMessage(_stateTransitionMessageSender);
    }

    void Update()
    {

    }
}
