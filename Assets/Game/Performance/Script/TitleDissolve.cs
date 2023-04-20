using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class TitleDissolve : MonoBehaviour
{
    [SerializeField, Tooltip("タイトルのCanvas")]
    private Canvas _titleCanvas;
    [SerializeField]
    private TitleController _titleController;
    [SerializeField, Tooltip("フェードするときのEvent")]
    private UnityEvent _onFade;

    /// <summary>ディゾルブさせるパネル</summary>
    private Image _dissolvePanel;
    /// <summary>ディゾルブのアニメーション</summary>
    private Animator _animator;

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

    /// <summary>親オブジェクトも含めて破壊する</summary>
    private void DestroyPanel()
    {
        _titleController.CurrentState = TitleController.TitleState.Menu;
        Destroy(this.transform.root.gameObject);
    }

    /// <summary>スクリーンショットを取ってアニメーションを呼んでいる</summary>
    /// <returns></returns>
    private IEnumerator Fade()
    {
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
