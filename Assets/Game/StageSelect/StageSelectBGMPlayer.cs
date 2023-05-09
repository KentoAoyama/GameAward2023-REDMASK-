// 日本語対応
using UnityEngine;

public class StageSelectBGMPlayer : MonoBehaviour
{
    private void Awake()
    {
        GameManager.Instance.AudioManager.PlayBGM("CueSheet_Gun", "BGM_Stage_Selection");
    }
}
