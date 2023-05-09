// 日本語対応
using System;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class ManualTextPresenter : MonoBehaviour
{
    [SerializeField]
    private PrepareDeviceManager _prepareDeviceManager = default;
    [SerializeField]
    private PrepareTextStringPair[] _prepareTextStringPairs = default;

    private void Awake()
    {
        _prepareDeviceManager.CurrentDevice.Subscribe(value =>
        {
            if (value == PrepareDevice.KeyboardAndMouse)
            {
                for (int i = 0; i < _prepareTextStringPairs.Length; i++)
                {
                    _prepareTextStringPairs[i].Text.text = _prepareTextStringPairs[i].KeyboardMouseText;
                }
            }
            else if (value == PrepareDevice.GamePad)
            {
                for (int i = 0; i < _prepareTextStringPairs.Length; i++)
                {
                    _prepareTextStringPairs[i].Text.text = _prepareTextStringPairs[i].GamePadText;
                }
            }
        });
    }
}
[Serializable]
public class PrepareTextStringPair
{
    [SerializeField]
    private Text _text = default;
    [SerializeField]
    private string _gamePadText = default;
    [SerializeField]
    private string _keyboardMouseText = default;

    public Text Text => _text;
    public string GamePadText => _gamePadText;
    public string KeyboardMouseText => _keyboardMouseText;
}
