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

    public bool SylinderEnabled
    {
        set 
        {
            _sylinderEnabled = value;
            Debug.Log($"_sylinder = {_sylinderEnabled}");
        }
    }

    private float _currentAngle = 0.0f;
    private bool _selectable = true;
    private bool _isRotating = false;
    private int _currentButtonIndex = 0;

    public bool Selectable
    {
        set => _selectable = value;
    }

    private void Awake()
    {
        _sylinderEnabled = false;
    }

    private void Update()
    {
        if ((Keyboard.current.aKey.wasPressedThisFrame ||
            Gamepad.current.dpad.left.isPressed ||
            Gamepad.current.leftStick.ReadValue().x < -0.1) &&
            _sylinderEnabled)
        {
            if (_isRotating || !_selectable) return;
            Debug.Log("A�L�[����������܂���");
            RotateCylinder(-60f);

            GameManager.Instance.AudioManager.PlaySE("CueSheet_Gun", "SE_Player_Reroad");

            _currentButtonIndex += 7;
            _currentButtonIndex %= 6;
            _circleDeploy.SelectButtons[_currentButtonIndex].Select();
        }

        if ((Keyboard.current.dKey.wasPressedThisFrame ||
            Gamepad.current.dpad.right.isPressed ||
            Gamepad.current.leftStick.ReadValue().x > 0.1) &&
            _sylinderEnabled)
        {
            if (_isRotating || !_selectable) return;
            RotateCylinder(60f);
            Debug.Log("D�L�[����������܂���");

            GameManager.Instance.AudioManager.PlaySE("CueSheet_Gun", "SE_Player_Reroad");

            _currentButtonIndex += 5;
            _currentButtonIndex %= 6;
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
