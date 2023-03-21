using UnityEngine;
using System;

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

    [Header("撃破された際のエフェクト")]
    [SerializeField] private EffectSettings _defeatedEffectSettings;
    [Header("発見時のエフェクト")]
    [SerializeField] private EffectSettings _discoverEffectSettings;
    [Header("調整用:生成した際にエフェクトを表示する")]
    [SerializeField] private bool _initActive;

    private GameObject _defeatedEffect;
    private GameObject _discoverEffect;

    private void Awake()
    {
        _defeatedEffect = Instantiate(_defeatedEffectSettings);
        _discoverEffect = Instantiate(_discoverEffectSettings);
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

    public void Discover()
    {
        _discoverEffect.SetActive(true);
    }

    public void Defeated()
    {
        _defeatedEffect.SetActive(true);
    }
}
