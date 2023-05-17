using UniRx;
using UnityEngine;

/// <summary>
/// プレイヤーの任意の行動をトリガーに敵を向かわせるクラス
/// 送る側
/// </summary>
public static class ReactionMessageSender
{
    public static void SendMessage(Transform transform)
    {
        MessageBroker.Default.Publish(new ReactionMessage(transform.position));
    }
}
