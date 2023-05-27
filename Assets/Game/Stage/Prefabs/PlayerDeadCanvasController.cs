// 日本語対応
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
using UniRx;

public class PlayerDeadCanvasController : MonoBehaviour
{
    [SerializeField]
    private GameObject _firstSelectedButton = default;

    [SerializeField]
    private GameObject _playerUIObject = default;

    [SerializeField]
    private float _fadeTime = 0.2f;

    private GameObject _preSelectedButton = default;

    private Graphic[] _graphics;
    private Button[] _buttons;

    private void OnEnable()
    {
        _graphics = GetComponentsInChildren<Graphic>();
        foreach (var graphic in _graphics)
        {
            if (graphic.gameObject.name != "Behind Obj Dont Touch Image")
            {
                graphic.color =
                    new Color(
                        graphic.color.r,
                        graphic.color.g,
                        graphic.color.b,
                        0f);
                graphic.DOFade(1f, _fadeTime);
            }
        }

        ButtonsInitilaize();
        GameManager.Instance.AudioManager.PlaySE("CueSheet_Gun", "ME_Death");
        _playerUIObject.SetActive(false);
        EventSystem.current.SetSelectedGameObject(_firstSelectedButton);
    }

    private void ButtonsInitilaize()
    {
        _buttons = GetComponentsInChildren<Button>();

        //子のボタンをすべて取得して、すべてにクリックした際の共通処理を登録する
        foreach (var button in _buttons)
        {
            button.OnClickAsObservable()
                .Subscribe(_ =>
                {
                    GameManager.Instance.AudioManager.PlaySE("CueSheet_Gun", "SE_Enter");
                    var graphics = button.GetComponentsInChildren<Graphic>();
                    foreach (var graphic in _graphics)
                    {
                        graphic.DOFade(0f, _fadeTime);
                    }
                    foreach (var button in _buttons)
                    {
                        button.interactable = false;
                    }
                });         
        }
    }

    private void OnDisable()
    {
        foreach (var graphic in _graphics)
        {
            if (graphic.gameObject.name != "Behind Obj Dont Touch Image")
            {
                DOTween.Kill(graphic.gameObject);
            }
        }
    }

    private void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            EventSystem.current.SetSelectedGameObject(_preSelectedButton);
        }
        _preSelectedButton = EventSystem.current.currentSelectedGameObject;
    }
}
