// 日本語対応
using UnityEngine;
using UnityEngine.UI;

public class SaveSoundSettingsButton : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<Button>().onClick.
            AddListener(GameManager.Instance.AudioManager.Save);
    }
}