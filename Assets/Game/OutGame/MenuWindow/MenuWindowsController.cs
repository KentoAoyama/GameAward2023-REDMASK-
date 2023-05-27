using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class MenuWindowsController : MonoBehaviour
{
    [SerializeField]
    private float _fadeTime = 1f;

    [SerializeField]
    private Button _closeSelectedTransitionButton = default;

    [SerializeField]
    private MenuWindowController _windowController = default;

    private Graphic[] _graphics;
    private LineRenderer[] _lines;

    [SerializeField]
    bool _isFade = false;


    private void Update()
    {
        if ((Keyboard.current != null && Keyboard.current.cKey.wasPressedThisFrame) ||
            (Gamepad.current != null && (Gamepad.current.startButton.wasPressedThisFrame || Gamepad.current.bButton.wasPressedThisFrame)))
        {
            Inactive();
        }
        if ((Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame) ||
            (Gamepad.current != null && (Gamepad.current.startButton.wasPressedThisFrame || Gamepad.current.bButton.wasPressedThisFrame)))
        {
            Inactive();
        }
    }

    private void OnDisable()
    {
        _isFade = false;
    }

    public void Active()
    {
        if (_isFade) return;
        GameManager.Instance.AudioManager.PlaySE("CueSheet_Gun", "SE_Enter");
        initilaize();
        StartCoroutine(ActiveCoroutine());
    }

    private IEnumerator ActiveCoroutine()
    {
        _isFade = true;

        foreach (var graphic in _graphics)
        {
            if (graphic.gameObject.name != "Behind obj dont touch image")
                graphic.DOFade(1f, _fadeTime);
        }
        yield return new WaitForSeconds(_fadeTime);
        if (_lines != null)
        {
            foreach (var line in _lines)
            {
                line.gameObject.SetActive(true);
            }
        }
        _isFade = false;
    }

    private void initilaize()
    {
        if (_graphics == null)
        {
            _graphics = gameObject.GetComponentsInChildren<Graphic>();
            foreach (var graphic in _graphics)
            {
                graphic.color =
                    new Color(
                        graphic.color.r,
                        graphic.color.g,
                        graphic.color.b,
                        0f);
            }
        }
        if (_lines == null)
        {
            _lines = gameObject.GetComponentsInChildren<LineRenderer>();

            if (_lines != null)
            {
                foreach (var line in _lines)
                {
                    line.gameObject.SetActive(false);
                }
            }
        }
    }

    public void Inactive()
    {
        if (_isFade) return;
        GameManager.Instance.AudioManager.PlaySE("CueSheet_Gun", "SE_Back");
        StartCoroutine(InactiveCoroutine());
        _windowController.Active();
    }

    private IEnumerator InactiveCoroutine()
    {
        _isFade = true;
        if (_lines != null)
        {
            foreach (var line in _lines)
            {
                line.gameObject.SetActive(false);
            }
        }
        foreach (var graphic in _graphics)
        {
            graphic.DOFade(0f, _fadeTime);
        }
        yield return new WaitForSeconds(_fadeTime);
        _isFade = false;
        EventSystem.current.SetSelectedGameObject(_closeSelectedTransitionButton.gameObject);
        gameObject.SetActive(false);
    }
}
