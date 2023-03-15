using UnityEngine;
using UniRx;

/// <summary>
/// 攻撃を行う際に使用するクラス
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
        Debug.Log("攻撃しました");
    }
}
