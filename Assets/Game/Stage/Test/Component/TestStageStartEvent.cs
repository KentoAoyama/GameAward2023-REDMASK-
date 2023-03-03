// 日本語対応
using Cysharp.Threading.Tasks;
using UnityEngine;

public class TestStageStartEvent : StageStartEventBase
{
    protected async override UniTask StartPerformance()
    {
        Debug.Log("ステージの開始演出を再生します");
        await UniTask.Delay(1000); // 警告を消すために待機処理を挿入
        Debug.Log("ステージの開始演出が完了しました");
    }
}
