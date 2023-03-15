using UnityEngine;
using UniRx;

/// <summary>
/// ���j���ꂽ
/// </summary>
public class DefeatedBehavior : MonoBehaviour
{
    void Awake()
    {
        MessageBroker.Default.Receive<BehaviorMessage>()
            .Where(message => message.ID == gameObject.GetInstanceID())
            .Where(message => message.Type == BehaviorType.Defeated)
            .Subscribe(_ => Defeated()).AddTo(this);
    }

    void Defeated()
    {
        Debug.Log("���j���ꂽ");
    }
}
