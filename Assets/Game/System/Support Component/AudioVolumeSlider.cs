// 日本語対応
using UnityEngine;
using UnityEngine.UI;

public class AudioVolumeSlider : MonoBehaviour
{
    [SerializeField]
    private AudioType _audioType = default;
    private void Awake()
    {
        // スライダーを取得
        var slider = GetComponent<Slider>();
        // スライダーの最大値と最小値を設定
        slider.minValue = 0f;
        slider.maxValue = 1f;
        // スライダーの値が変わった時に呼ばれる処理を登録
        switch (_audioType)
        {
            case AudioType.Master:
                slider.onValueChanged.AddListener(GameManager.Instance.AudioManager.ChangeMasterVolume);
                break;
            case AudioType.BGM:
                slider.onValueChanged.AddListener(GameManager.Instance.AudioManager.ChangeBGMVolume);
                break;
            case AudioType.SE:
                slider.onValueChanged.AddListener(GameManager.Instance.AudioManager.ChangeSEVolume);
                break;
        }
    }
}
[SerializeField]
public enum AudioType
{
    Master, BGM, SE
}
