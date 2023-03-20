using UnityEngine;
using System;

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

    [Header("���j���ꂽ�ۂ̃G�t�F�N�g")]
    [SerializeField] private EffectSettings _defeatedEffectSettings;
    [Header("�������̃G�t�F�N�g")]
    [SerializeField] private EffectSettings _discoverEffectSettings;
    [Header("�����p:���������ۂɃG�t�F�N�g��\������")]
    [SerializeField] private bool _initActive;

    private GameObject _defeatedEffect;
    private GameObject _discoverEffect;

    private void Awake()
    {
        _defeatedEffect = Instantiate(_defeatedEffectSettings);
        _discoverEffect = Instantiate(_discoverEffectSettings);
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

    public void Discover()
    {
        _discoverEffect.SetActive(true);
    }

    public void Defeated()
    {
        _defeatedEffect.SetActive(true);
    }
}
