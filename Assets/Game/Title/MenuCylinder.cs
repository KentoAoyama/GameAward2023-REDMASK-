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

    private float _currentAngle = 0.0f;
    private bool _selectable = true;
    private bool _isRotating = false;
    private int _currentButtonIndex = 0;

    public bool Selectable
    {
        set => _selectable = value;
    }

    private void Update()
    {
        
        if (Keyboard.current.aKey.wasPressedThisFrame)
        {
            if (_isRotating || !_selectable) return;
            Debug.Log("Aキーが押下されました");
            RotateCylinder(-60f);

            GameManager.Instance.AudioManager.PlaySE("CueSheet_Gun", "SE_Player_Reroad");

            _currentButtonIndex += 7;
            _currentButtonIndex %= 6;
            _circleDeploy.SelectButtons[_currentButtonIndex].Select();
        }

        if (Keyboard.current.dKey.wasPressedThisFrame)
        {
            if (_isRotating || !_selectable) return;
            RotateCylinder(60f);
            Debug.Log("Dキーが押下されました");

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
