// 日本語対応
using UnityEngine;
using UnityEngine.UI;

public class RevivalButton : MonoBehaviour
{
    [SerializeField]
    private StageStartMode _revivalMode = default;
    [SerializeField]
    private RevivalButtonManager _revivalButtonManager = default;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() => _revivalButtonManager.StageController2.Revival(_revivalMode));
    }
}