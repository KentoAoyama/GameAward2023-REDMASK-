using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UniRx;

public class TGSGoToStageButton : MonoBehaviour
{
    [SerializeField]
    private StageTypeStageNamePair _tgsStage = default;
    [SerializeField]
    private MaskSwitch _maskSwitch = default;
    [SerializeField]
    private Color _disableColor = Color.white;
    [SerializeField]
    private BulletPrepareControl _bulletPrepareControl = default;

    private Image _image = null;
    
    private void Start()
    {
        _image = GetComponent<Image>();
        var button = GetComponent<Button>();
        _maskSwitch.IsSet.Subscribe(value =>
        {
            button.enabled = value;
            _image.color = value ? Color.white : _disableColor;
        });
        button.onClick.AddListener(GoToStage);
    }

    [SerializeField] private PrepareFadeOut _prepareFadeOut = default;
    private bool _isPlaying = false;

    public async void GoToStage()
    {
        if (_isPlaying) return;
        _isPlaying = true;

        await _prepareFadeOut.FadeOut();

        SceneManager.LoadScene(_tgsStage.SceneName);
        _bulletPrepareControl?.AssignBulletsCount();
    }
}
