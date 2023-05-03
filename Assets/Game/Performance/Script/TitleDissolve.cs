using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class TitleDissolve : MonoBehaviour
{
    [SerializeField, Tooltip("�^�C�g����Canvas")]
    private Canvas _titleCanvas;
    [SerializeField]
    private TitleController _titleController;
    [SerializeField, Tooltip("�t�F�[�h����Ƃ���Event")]
    private UnityEvent _onFade;
    [SerializeField, Tooltip("Dissolveが終わった時のコールバック")]
    private UnityEvent _fadeCollback;

    /// <summary>�f�B�]���u������p�l��</summary>
    private Image _dissolvePanel;
    /// <summary>�f�B�]���u�̃A�j���[�V����</summary>
    private Animator _animator;

    private bool _isFading = false;
    private void Awake()
    {
        _dissolvePanel = GetComponent<Image>();
        _animator = GetComponent<Animator>();
        _dissolvePanel.enabled = false;
    }

    private void Update()
    {
        if (Keyboard.current.anyKey.wasPressedThisFrame)
        {
            StartCoroutine(Fade());
        }
    }

    /// <summary>�e�I�u�W�F�N�g���܂߂Ĕj�󂷂�</summary>
    private void DestroyPanel()
    {
        _titleController.CurrentState = TitleController.TitleState.Menu;
        Destroy(this.transform.root.gameObject);
        _fadeCollback.Invoke();
    }

    /// <summary>�X�N���[���V���b�g������ăA�j���[�V�������Ă�ł���</summary>
    /// <returns></returns>
    private IEnumerator Fade()
    {
        if (_isFading)
        {
            yield break;
        }

        _isFading = true;

        yield return new WaitForEndOfFrame();

        Texture2D screenShot = new Texture2D(Screen.width, Screen.height);
        screenShot.ReadPixels(new Rect(0f, 0f, Screen.width, Screen.height), 0, 0);
        screenShot.Apply();

        _onFade.Invoke();

        _dissolvePanel.enabled = true;
        _dissolvePanel.sprite = Sprite.Create(screenShot, new Rect(0f, 0f, screenShot.width, screenShot.height), Vector2.zero);
        _animator.Play("TitleDissolvePlay");

        Destroy(_titleCanvas.gameObject);
    }
}
