using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class MenuPanelController : MonoBehaviour
{
    [SerializeField, Tooltip("このパネルのアニメーションを管理するパラメータ名")]
    private string _animationParameterName = "";
    [SerializeField]
    private MenuCylinder _cylinder = default;
    [SerializeField]
    private Button _button = default;

    private bool _panelEnabled = false;

    public bool PanelEnabled
    {
        set => _panelEnabled = value;
    }

    private bool _cansellable = false;

    public bool Cansellable
    {
        set => _cansellable = value;
    }
    private Animator _canvasAnimator;


    private void Awake()
    {
        _canvasAnimator = transform.parent.gameObject.GetComponent<Animator>();
        _panelEnabled = false;
        _cansellable = false;
    }

    private void Update()
    {
        if (!_panelEnabled) return;

        if (_cansellable)
        {
            // Close
            bool close = Keyboard.current.cKey.wasPressedThisFrame;

            if (Gamepad.current != null)
            {
                close |= Gamepad.current.bButton.wasPressedThisFrame;
            }

            if (close)
            {
                ClosePanel();
                GameManager.Instance.AudioManager.PlaySE("CueSheet_Gun", "SE_Back");
                _cylinder.SylinderEnabled = true;
                _button.Select();
            }
        }
    }

    public void OpenPanel()
    {
        if (_panelEnabled) return;

        _canvasAnimator.SetBool(_animationParameterName, true);
        EventSystem.current.SetSelectedGameObject(null);
    }

    private void ClosePanel()
    {
        if (!_panelEnabled) return;

        _canvasAnimator.SetBool(_animationParameterName, false);
        EventSystem.current.SetSelectedGameObject(null);
        GameManager.Instance.AudioManager.Save();
        _panelEnabled = false;
    }
}
