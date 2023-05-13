// 日本語対応
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class StageSelectManualTextPresenter : MonoBehaviour
{
    [SerializeField]
    private Text _manualText = default;
    [SerializeField]
    private PrepareDeviceManager _deviceManager = default;

    private void Awake()
    {
        _deviceManager.CurrentDevice.Subscribe(value =>
        {
            if (value == PrepareDevice.GamePad)
            {
                _manualText.text = "A : 決定";
            }
            else
            {
                _manualText.text = "Enter : 決定";
            }
        });
    }
}
