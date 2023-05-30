using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UniRx;

public class CursorController : System.IDisposable
{
    enum Controller
    {
        KeyboardMouse,
        Gamepad
    }

    private ReactiveProperty<Controller> _controllerProp = new ReactiveProperty<Controller>(Controller.KeyboardMouse);
    private InputAction _keyboardAction = new InputAction(binding: "<Keyboard>/*");
    private InputAction _mouseAction = new InputAction(binding: "<Mouse>/*");
    private InputAction _GamepadAction = new InputAction(binding: "<Gamepad>/*");
    private CompositeDisposable _disposable = new CompositeDisposable();

    public CursorController()
    {
        _controllerProp.Subscribe(CursorControl).AddTo(_disposable);
        Cursor.lockState = CursorLockMode.Confined;

        _keyboardAction.Enable();
        _keyboardAction.started += _ => _controllerProp.Value = Controller.KeyboardMouse;
        _mouseAction.Enable();
        _mouseAction.started += _ => _controllerProp.Value = Controller.KeyboardMouse;
        _GamepadAction.Enable();
        _GamepadAction.started += _ => _controllerProp.Value = Controller.Gamepad;
    }

    ~CursorController()
    {
        Dispose();
    }

    private void CursorControl(Controller controller)
    {
        if (controller == Controller.Gamepad)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
        }
    }

    public void Dispose()
    {
        _disposable.Dispose();
    }
}
