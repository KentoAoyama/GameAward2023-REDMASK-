// 日本語対応
using UnityEngine;

/// <summary>
/// ボタンから機能をテストする用のコンポーネント
/// </summary>
public class StageTestButton : MonoBehaviour
{
    [SerializeField]
    private StageController _stageController = default;

    public void OnPlayerDead()
    {
        _stageController.PlayerDead();
    }
    public void OnStageComplete()
    {
        _stageController.StageComplete();
    }
}
