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
        // ガンベルトの状態を保存
        GameManager.Instance.BulletsCountManager.BulletCountStage[BulletType.StandardBullet].Value = 4;
        GameManager.Instance.BulletsCountManager.BulletCountStage[BulletType.PenetrateBullet].Value = 2;

        // シリンダーの状態を保存
        BulletType[] bullets = new BulletType[6];
        for (int i = 0; i < bullets.Length; i++)
        {
            bullets[i] = BulletType.StandardBullet;
        }
        GameManager.Instance.BulletsCountManager.Cylinder = bullets;
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
        _manualText.enabled = true;
        _manualText.DOFade(0.2f, 1.5f).SetLoops(-1, LoopType.Yoyo);
    }
}
