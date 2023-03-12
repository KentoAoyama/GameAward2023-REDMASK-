using UnityEngine;

/// <summary>
/// �G�̊e�@�\�̒�����s���N���X
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
