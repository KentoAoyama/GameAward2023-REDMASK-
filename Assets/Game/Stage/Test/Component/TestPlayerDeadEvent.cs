// 日本語対応
using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TestPlayerDeadEvent : PlayerDeadEventBase
{
    protected async override UniTask PlayerDeadPerformance()
    {
        Debug.Log("プレイヤーの死亡演出を再生します");
        await UniTask.Delay(1000); // 警告を消すために待つ処理を挿入
        Debug.Log("プレイヤーの死亡演出完了");
    }
}
