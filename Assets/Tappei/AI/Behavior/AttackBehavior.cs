using UnityEngine;
using UniRx;

/// <summary>
/// �U�����s���ۂɎg�p����N���X
/// </summary>
public class AttackBehavior : MonoBehaviour
{
    void Awake()
    {
        MessageBroker.Default.Receive<BehaviorMessage>()
            .Where(message => message.ID == gameObject.GetInstanceID())
            .Where(message => message.Type == BehaviorType.Attack)
            .Subscribe(_ => Attack()).AddTo(this);
    }

    void Attack()
    {
        Debug.Log("�U�����܂���");
    }
}
