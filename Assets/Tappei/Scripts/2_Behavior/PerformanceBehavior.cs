using UnityEngine;
using System;
using DG.Tweening;

/// <summary>
/// 各種演出を行うクラス
/// </summary>
public class PerformanceBehavior : MonoBehaviour
{
    [Serializable]
    private struct EffectSettings
    {
        [SerializeField] private GameObject _prefab;
        [Tooltip("キャラクターの足元を基準に設定する")]
        [SerializeField] private Vector2 _offset;

        public GameObject Prefab => _prefab;
        public Vector2 Offset => _offset;
    }

    static readonly float DefeatedEffectLifeTime = 1.5f;

    [Header("撃破された際のエフェクト")]
    [SerializeField] private EffectSettings _defeatedEffectSettings;
    [Header("発見時のエフェクト")]
    [SerializeField] private EffectSettings _discoverEffectSettings;
    [Header("調整用:生成した際にエフェクトを表示する")]
    [SerializeField] private bool _initActive;

    /// <summary>
    /// 発見時のエフェクトは使いまわすのでメンバとして保持しておく
    /// </summary>
    private GameObject _discoverEffect;

    private void Awake()
    {
        _discoverEffect = Instantiate(_discoverEffectSettings);
    }

    public void Discover() => _discoverEffect.SetActive(true);

    /// <summary>
    /// キャラクターの向きに合わせて生成する必要があるのでSpriteの向きが必要
    /// </summary>
    public void Defeated(int dir)
    {
        GameObject instance = Instantiate(_defeatedEffectSettings);
        instance.transform.parent = null;
        DOVirtual.DelayedCall(DefeatedEffectLifeTime, () => instance.SetActive(false))
            .OnStart(() => instance.SetActive(true)).SetLink(gameObject);

        Vector3 scale = Vector3.one;
        scale.x = dir;
        instance.transform.localScale = scale;
    }

    /// <summary>
    /// 各種エフェクトの生成処理
    /// Prefabがnullだった場合は適当なGameObjectを作って返す
    /// </summary>
    private GameObject Instantiate(EffectSettings effectSettings)
    {
        GameObject prefab = effectSettings.Prefab;
        if (!prefab) return new GameObject();

        Vector2 pos = transform.position + (Vector3)effectSettings.Offset;
        GameObject instance = Instantiate(prefab, pos, Quaternion.identity, transform);
        instance.SetActive(_initActive);

        return instance;
    }
}
