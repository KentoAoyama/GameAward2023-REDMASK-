using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bullet;
using UnityEngine.UI;
using DG.Tweening;

public class PrepareTutorialController : MonoBehaviour
{
    [SerializeField]
    private GameObject _tutolialEvent;

    [SerializeField]
    private GameObject _cutSceneObject;

    [SerializeField]
    private PrepareFadeOut _fadeOut;

    [SerializeField]
    private BulletPrepareControl _bulletPrepareControl;

    [SerializeField]
    private Button _maskButton;

    [SerializeField]
    private Text _manualText;

    private void Start()
    {
        _tutolialEvent.SetActive(false);
        _cutSceneObject.SetActive(true);
    }

    private void Update()
    {
        if (!_cutSceneObject.activeSelf)
        {
            _tutolialEvent.SetActive(true);
        }
    }

    public void FadeOut()
    {
        _fadeOut.FadeOut().Forget();
    }

    public void PushStandardBullet()
    {
        _bulletPrepareControl.PushBullet(BulletType.StandardBullet);
        GameManager.Instance.AudioManager.PlaySE("CueSheet_Gun", "SE_Bullets_Selection");
    }

    public void PushReflectBullet()
    {
        _bulletPrepareControl.PushBullet(BulletType.ReflectBullet);
        GameManager.Instance.AudioManager.PlaySE("CueSheet_Gun", "SE_Bullets_Selection");
    }

    public void PushPenetrateBullet()
    {
        _bulletPrepareControl.PushBullet(BulletType.PenetrateBullet);
        GameManager.Instance.AudioManager.PlaySE("CueSheet_Gun", "SE_Bullets_Selection");
    }

    public void GetMask()
    {
        _maskButton.onClick.Invoke();
        _bulletPrepareControl.AssignBulletsCount();
    }

    public void ManualTextTween()
    {
        _manualText.DOFade(0.2f, 1.5f).SetLoops(-1, LoopType.Yoyo);
    }
}
