// 日本語対応
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;
using UniRx;
using System.Collections;
using UnityEngine.Rendering;
using DG.Tweening;
using UnityEngine.InputSystem;

public class MenuWindowController : MonoBehaviour
{
    [SerializeField]
    private GameObject _firstSelectedObject = default;

    [SerializeField]
    private GameObject _playerUIObject = default;

    [Header("MainUI")]

    [SerializeField]
    private GameObject _mainUIObject = default;

    [SerializeField]
    private float _fadeMoveTime = 0.2f;

    [SerializeField]
    private float _fadeColorTime = 0.05f;

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

    private Graphic[] _graphics;

    private bool _isFade = false;
    public bool IsFade => _isFade;

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

        _resumeButton.OnClickAsObservable()
            .Subscribe(_ =>
            {
                StartCoroutine(WindowClose());
            });
        _manualButton.OnClickAsObservable()
            .Subscribe(_ =>
            {
                _manualWindow.gameObject.SetActive(true);
                _manualWindow.Active();
                Inactive();
            });
        _audioSettingButton.OnClickAsObservable()
            .Subscribe(_ =>
            {
                _audioWindow.gameObject.SetActive(true);
                _audioWindow.Active();
                Inactive();
            });
        _goToTitleButton.OnClickAsObservable()
            .Subscribe(_ =>
            {
                _goToTitleWindow.gameObject.SetActive(true);
                _goToTitleWindow.Active();
                Inactive();
            });
        _applicationQuitButton.OnClickAsObservable()
            .Subscribe(_ =>
            {
                _applicationQuitWindow.gameObject.SetActive(true);
                _applicationQuitWindow.Active();
                Inactive();
            });
    }

    private void OnEnable()
    {
        Active();

        //インゲームののUIを非表示にする
        if (_playerUIObject != null)
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
        if (_playerUIObject != null)
            _playerUIObject.SetActive(true);
    }

    public void Active()
    {
        StartCoroutine(ActiveCoroutine());
    }

    private IEnumerator ActiveCoroutine()
    {
        if (_isFade) yield break;

        _isFade = true;
        if (_graphics == null)
        {
            _graphics = _mainUIObject.GetComponentsInChildren<Graphic>();
            foreach (var graphic in _graphics)
            {
                graphic.color =
                    new Color(
                        graphic.color.r,
                        graphic.color.g,
                        graphic.color.b,
                        0f);
            }
            _mainUIObject.transform.localPosition = new Vector3(-800f, 0f, 0f);
        }

        foreach (var graphic in _graphics)
        {
            if (graphic.gameObject.name != "Behind obj dont touch image")

                graphic.DOFade(1f, _fadeMoveTime);

        }
        _mainUIObject.transform.DOLocalMoveX(0f, _fadeColorTime);
        yield return new WaitForSeconds(_fadeMoveTime);
        _isFade = false;
    }

    private void Inactive()
    {
        StartCoroutine(InactiveCoroutine());
    }

    private IEnumerator InactiveCoroutine()
    {
        if (_isFade) yield break;

        _isFade = true;
        foreach (var graphic in _graphics)
        {
            graphic.DOFade(0f, _fadeMoveTime);
        }
        _mainUIObject.transform.DOLocalMoveX(-800f, _fadeColorTime);
        yield return new WaitForSeconds(_fadeMoveTime);
        _isFade = false;
    }

    private void Update()
    {
        // 操作デバイスを更新する
        if (PrepareDeviceManager.MonitorInput() != PrepareDevice.None)
        {
            CurrentDevice = PrepareDeviceManager.MonitorInput();
        }

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

        //Windowを閉じる入力判定
        if ((Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame) ||
            (Gamepad.current != null && Gamepad.current.startButton.wasPressedThisFrame))
        {
            StartCoroutine(WindowClose());
        }
    }

    private IEnumerator WindowClose()
    {
        if (_isFade) yield break;

        GameManager.Instance.AudioManager.PlaySE("CueSheet_Gun", "SE_Back");
        yield return InactiveCoroutine();
        gameObject.SetActive(false);
    }
}
