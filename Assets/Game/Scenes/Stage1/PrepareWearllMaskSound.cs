// 日本語対応
using UnityEngine;
using UnityEngine.UI;

public class PrepareWearllMaskSound : MonoBehaviour
{
    public void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        GameManager.Instance.AudioManager.PlaySE("CueSheet_Gun", "SE_Mask"));
    }
}
