using UniRx;
using UnityEngine;

public class DebugUIButtonControl : MonoBehaviour
{
    public void PublishEnemyStateControlMessage()
    {
        MessageBroker.Default.Publish(StateTransitionTrigger.TimeElapsed);
    }
}
