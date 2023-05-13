// 日本語対応
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UnityEngine.EventSystems;

/// <summary>
/// 弾を減算する用のボタン
/// </summary>
public class BulletSubtractionButton : MonoBehaviour
{
    [SerializeField]
    private BulletPrepareControl _bulletPrepareControl = default;
    [SerializeField]
    private EventSystem _eventSystem = default;
    [SerializeField]
    private PrepareUIController _uiController = default;
    [SerializeField]
    private int _index = default;
    [SerializeField]
    private StorageSiteType _storageSiteType = default;

    private void Awake()
    {
        var button = GetComponent<Button>();
        button.onClick.AddListener(BulletSubtraction);

        if (_storageSiteType == StorageSiteType.Cylinder)
        {
            _bulletPrepareControl.Cylinder[_index].
                Subscribe(value => button.interactable = value != Bullet.BulletType.NotSet);
        }
        else if (_storageSiteType == StorageSiteType.GunBelt)
        {
            _bulletPrepareControl.GunBelt[_index].
                Subscribe(value => button.interactable = value != Bullet.BulletType.NotSet);
        }
    }
    private void BulletSubtraction()
    {
        GameManager.Instance.AudioManager.PlaySE("CueSheet_Gun", "SE_Bullets_Selection");
        _bulletPrepareControl.PullBullet(_storageSiteType, _index);
        GameObject a = null;
        if (_storageSiteType == StorageSiteType.Cylinder)
        {
            a = _uiController.GetNearSelectableObjForCylinder(transform);
        }
        else if (_storageSiteType == StorageSiteType.GunBelt)
        {
            a = _uiController.GetNearSelectableObjForGunbelt(transform);
        }
        _eventSystem.SetSelectedGameObject(a);
    }
}
