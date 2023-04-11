// 日本語対応
using System.Collections.Generic;
using Bullet;
using UnityEngine;
using UniRx;

public class CheckPoint : MonoBehaviour
{
    [Header("蘇生座標")]
    [SerializeField]
    private Transform _revivePosition = default;
    public Transform RevivePosition => _revivePosition;

    private void Awake()
    {
        GetComponent<Collider2D>().isTrigger = true;
    }
    /// <summary>
    /// このチェックポイントが生きているかどうか
    /// </summary>
    public bool IsAlive { get; set; } = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Player.PlayerController player) && IsAlive)
        {
            IsAlive = false;
            gameObject.SetActive(false);
            // シリンダーの状態を保存する。（同じインスタンスを参照するわけには行かないので、参照先インスタンスの複製を作成する。）
            var cylinder = new IStoreableInChamber[player.Revolver.Cylinder.Length];
            for (int i = 0; i < cylinder.Length; i++)
            {
                cylinder[i] = player.Revolver.Cylinder[i];
            }
            // ガンベルトの状態を保存する。（同じインスタンスを参照するわけには行かないので、参照先インスタンスの複製を作成する。）
            var gunbelt = new Dictionary<BulletType, IReadOnlyReactiveProperty<int>>(player.BulletCountManager.BulletCounts);
            gunbelt[BulletType.StandardBullet] =
                new ReactiveProperty<int>(player.BulletCountManager.BulletCounts[BulletType.StandardBullet].Value);
            gunbelt[BulletType.PenetrateBullet] =
                new ReactiveProperty<int>(player.BulletCountManager.BulletCounts[BulletType.PenetrateBullet].Value);
            gunbelt[BulletType.ReflectBullet] =
                new ReactiveProperty<int>(player.BulletCountManager.BulletCounts[BulletType.ReflectBullet].Value);

            GameManager.Instance.StageManager.TouchCheckPoint(_revivePosition.position, cylinder, gunbelt);
        }
    }
}
