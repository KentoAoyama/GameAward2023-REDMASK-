using UnityEngine;
using System;
using DG.Tweening;

/// <summary>
/// �e�퉉�o���s���N���X
/// </summary>
public class PerformanceBehavior : MonoBehaviour
{
    [Serializable]
    private struct EffectSettings
    {
        [SerializeField] private GameObject _prefab;
        [Tooltip("�L�����N�^�[�̑�������ɐݒ肷��")]
        [SerializeField] private Vector2 _offset;

        public GameObject Prefab => _prefab;
        public Vector2 Offset => _offset;
    }

    static readonly float DefeatedEffectLifeTime = 1.5f;

    [Header("���j���ꂽ�ۂ̃G�t�F�N�g")]
    [SerializeField] private EffectSettings _defeatedEffectSettings;
    [Header("�������̃G�t�F�N�g")]
    [SerializeField] private EffectSettings _discoverEffectSettings;
    [Header("�����p:���������ۂɃG�t�F�N�g��\������")]
    [SerializeField] private bool _initActive;

    /// <summary>
    /// �������̃G�t�F�N�g�͎g���܂킷�̂Ń����o�Ƃ��ĕێ����Ă���
    /// </summary>
    private GameObject _discoverEffect;

    private void Awake()
    {
        _discoverEffect = Instantiate(_discoverEffectSettings);
    }

    public void Discover() => _discoverEffect.SetActive(true);

    /// <summary>
    /// �L�����N�^�[�̌����ɍ��킹�Đ�������K�v������̂�Sprite�̌������K�v
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
    /// �e��G�t�F�N�g�̐�������
    /// Prefab��null�������ꍇ�͓K����GameObject������ĕԂ�
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
