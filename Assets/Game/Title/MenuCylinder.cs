using DG.Tweening;
using UnityEngine.InputSystem;
using UnityEngine;
using Unity.VisualScripting;
using UnityEngine.UI;

public class MenuCylinder : MonoBehaviour
{
    [SerializeField]
    private CircleDeploy _circleDeploy;
    [SerializeField]
    private Transform _cylinderImage;
    [SerializeField]
    private TitleController _titleController;
    [SerializeField, Tooltip("シリンダーが回るかどうか")]
    private bool _sylinderEnabled = false;
    /// <summary>
    /// シリンダーが回るかどうか
    /// </summary>
    /// <value></value>
    public bool SylinderEnabled
    {
        set 
        {
            _sylinderEnabled = value;
        }
    }

    private float _currentAngle = 0.0f;
    private bool _isRotating = false;
    private int _currentButtonIndex = 0;

    private void Awake()
    {
        _sylinderEnabled = false;
    }

    private void Update()
    {
        bool left = Keyboard.current.aKey.wasPressedThisFrame || Keyboard.current.leftArrowKey.wasPressedThisFrame;
        bool right = Keyboard.current.dKey.wasPressedThisFrame || Keyboard.current.rightArrowKey.wasPressedThisFrame;

        if (Gamepad.current != null)
        {
            left |= Gamepad.current.dpad.left.isPressed || Gamepad.current.leftStick.ReadValue().x < -0.1;
            right |= Gamepad.current.dpad.right.isPressed || Gamepad.current.leftStick.ReadValue().x > 0.1;
        }

        if (left && _sylinderEnabled && !_isRotating)
        {
            GameManager.Instance.AudioManager.PlaySE("CueSheet_Gun", "SE_Selection");

            _currentButtonIndex += 7;
            _currentButtonIndex %= 6;
            int skipCount = 1;

            while (!_circleDeploy.SelectButtons[_currentButtonIndex].enabled)
            {
                _currentButtonIndex += 7;
                _currentButtonIndex %= 6;
                skipCount++;
            }

            RotateCylinder(-60f * skipCount);
            _circleDeploy.SelectButtons[_currentButtonIndex].Select();
        }
        else if (right && _sylinderEnabled && !_isRotating)
        {
            GameManager.Instance.AudioManager.PlaySE("CueSheet_Gun", "SE_Selection");

            _currentButtonIndex += 5;
            _currentButtonIndex %= 6;
            int skipCount = 1;

            while (!_circleDeploy.SelectButtons[_currentButtonIndex].enabled)
            {
                _currentButtonIndex += 5;
                _currentButtonIndex %= 6;
                skipCount++;
            }

            RotateCylinder(60f * skipCount);
            _circleDeploy.SelectButtons[_currentButtonIndex].Select();
        }
    }

    public void RotateCylinder(float angle)
    {
        _isRotating = true;
        _currentAngle += angle;
        _currentAngle %= 360f;
        _circleDeploy.RotateChild(angle);
        _cylinderImage.DOLocalRotate(new Vector3(0f, 0f, _currentAngle), 0.5f).
            OnComplete(() => _isRotating = false);
    }
}
