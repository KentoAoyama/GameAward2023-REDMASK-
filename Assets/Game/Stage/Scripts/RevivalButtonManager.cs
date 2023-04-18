// 日本語対応
using UnityEngine;

public class RevivalButtonManager : MonoBehaviour
{
    [SerializeField]
    private StageController2 _stageController2 = default;

    public StageController2 StageController2 => _stageController2;
}