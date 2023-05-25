// 日本語対応
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;
using UniRx;

public class MenuWindowController : MonoBehaviour
{
    [SerializeField]
    private GameObject _firstSelectedObject = default;

    [SerializeField]
    private GameObject _playerUIObject = default;

    [Header("Buttons")]

    [SerializeField]
    private Button _resumeButton = default;

    [SerializeField]
    private Button _manualButton = default;

    [SerializeField]
    private Button _audioSettingButton = default;

    [SerializeField]
    private Button _goToTitleButton = default;

    [SerializeField]
    private Button _applicationQuitButton = default;


    [Header("Windows")]

    [SerializeField]
    private MenuWindowsController _manualWindow = default;

    [SerializeField]
    private MenuWindowsController _audioWindow = default;

    [SerializeField]
    private MenuWindowsController _goToTitleWindow = default;

    [SerializeField]
    private MenuWindowsController _applicationQuitWindow = default;


    private List<Button> _mainButtons = new();
    private GameObject _previousSelectedObject = null;

    public static PrepareDevice CurrentDevice { get; private set; } = PrepareDevice.GamePad;


    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        //まとめて処理を実行しやすくするため、ボタンをすべてリストに追加しておく
        _mainButtons.Add(_resumeButton);
        _mainButtons.Add(_manualButton);
        _mainButtons.Add(_audioSettingButton);
        _mainButtons.Add(_goToTitleButton);
        _mainButtons.Add(_applicationQuitButton);


        _manualButton.OnClickAsObservable()
            .Subscribe(_ => 
            {
                _manualWindow.gameObject.SetActive(true);
                _manualWindow.Active();
            });
        _audioSettingButton.OnClickAsObservable()
            .Subscribe(_ =>
            {
                _audioWindow.gameObject.SetActive(true);
                _audioWindow.Active();
            });
        _goToTitleButton.OnClickAsObservable()
            .Subscribe(_ => 
            {
                _goToTitleWindow.gameObject.SetActive(true);
                _goToTitleWindow.Active();
            });
        _applicationQuitButton.OnClickAsObservable()
            .Subscribe(_ => 
            {
                _goToTitleWindow.gameObject.SetActive(true);
                _goToTitleWindow.Active();
            });
    }

    private void OnEnable()
    {
        //インゲームののUIを非表示にする
        _playerUIObject.SetActive(false);

        EventSystem.current.SetSelectedGameObject(_firstSelectedObject);
        _previousSelectedObject = _firstSelectedObject;
        if (_firstSelectedObject.TryGetComponent(out Outline currentOutline))
        {
            currentOutline.enabled = true;
        }
    }
    private void OnDisable()
    {
        for (int i = 0; i < _mainButtons.Count; i++)
        {
            if (_mainButtons[i].TryGetComponent(out Outline outline))
            {
                outline.enabled = false;
            }
        }

        //インゲームののUIを表示する
        _playerUIObject.SetActive(true);
    }
    private void Update()
    {
        // 操作デバイスを更新する
        CurrentDevice = PrepareDeviceManager.MonitorInput();

        if (EventSystem.current.currentSelectedGameObject == null)
        {
            EventSystem.current.SetSelectedGameObject(_previousSelectedObject);
        }
        if (EventSystem.current.currentSelectedGameObject != _previousSelectedObject)
        {
            if (EventSystem.current.currentSelectedGameObject.TryGetComponent(out Outline currentOutline))
            {
                currentOutline.enabled = true;
            }
            if (_previousSelectedObject.TryGetComponent(out Outline previousOutline))
            {
                previousOutline.enabled = false;
            }
        }
        _previousSelectedObject = EventSystem.current.currentSelectedGameObject;
    }

    public void EnableMainButtons()
    {
        for (int i = 0; i < _mainButtons.Count; i++)
        {
            _mainButtons[i].interactable = true;
        }
    }
    public void DisableMainButtons()
    {
        for (int i = 0; i < _mainButtons.Count; i++)
        {
            _mainButtons[i].interactable = false;
        }
    }
}
