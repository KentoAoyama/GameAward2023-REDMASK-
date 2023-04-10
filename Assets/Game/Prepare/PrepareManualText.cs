// 日本語対応
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class PrepareManualText : MonoBehaviour
{
    [SerializeField]
    private MaskSwitch _maskSwitch = default;
    [SerializeField]
    private string _notSetText = default;
    [SerializeField]
    private string _setText = default;

    private Text _manual = null;

    private void Awake()
    {
        _manual = GetComponent<Text>();
        _maskSwitch.IsSet.Subscribe(value => _manual.text = value ? _setText : _notSetText);
    }
}
