// 日本語対応
using Cysharp.Threading.Tasks;
using UnityEngine;

public class TestStageCompleteEvent : StageCompleteEventBase
{
    protected async override UniTask CompletePerformance()
    {
        Debug.Log("ステージのクリア演出を再生します");
        await UniTask.Delay(1000); // 警告を消すために待つ処理を挿入
        Debug.Log("ステージクリア演出再生完了");
    }
}
