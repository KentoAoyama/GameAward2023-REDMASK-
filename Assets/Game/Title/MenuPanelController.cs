using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class MenuPanelController : MonoBehaviour
{
    [SerializeField, Tooltip("このパネルのアニメーションを管理するパラメータ名")]
    private string _animationParameterName = "";
    [SerializeField, Tooltip("partcleのアニメーション")]
    private TitleParticleAnimation _particleAnim = default;

    private bool _panelEnabled = false;

    public bool PanelEnabled
    {
        set => _panelEnabled = value;
    }

    private Animator _canvasAnimator;


    private void Awake()
    {
        _canvasAnimator = transform.parent.gameObject.GetComponent<Animator>();
        _panelEnabled = false;
    }

    private void Update()
    {
        if (!_panelEnabled) return;

        // Close
        bool close = Keyboard.current.cKey.wasPressedThisFrame;

        if (Gamepad.current != null)
        {
            close |= Gamepad.current.bButton.wasPressedThisFrame;
        }

        if (close)
        {
            ClosePanel();
        }
    }

    public void OpenPanel()
    {
        if (_panelEnabled) return;

        _canvasAnimator.SetBool(_animationParameterName, true);
        EventSystem.current.SetSelectedGameObject(null);
        _particleAnim.AnimPlay();
    }

    private void ClosePanel()
    {
        if (!_panelEnabled) return;

        _canvasAnimator.SetBool(_animationParameterName, false);
        EventSystem.current.SetSelectedGameObject(null);
        _panelEnabled = false;
        _particleAnim.AnimPlay();
    }
}
