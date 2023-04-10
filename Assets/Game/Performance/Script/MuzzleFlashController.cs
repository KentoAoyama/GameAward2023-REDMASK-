using UnityEngine;
using UnityEngine.Rendering.Universal;

public class MuzzleFlashController : MonoBehaviour
{
    /// <summary>�A�j���[�V����������Particle</summary>
    private ParticleSystem _particleSystem;
    /// <summary>�A�j���[�V����������Light2D</summary>
    private SpriteRenderer _renderer;

    private void Awake()
    {
        _particleSystem = GetComponentInChildren<ParticleSystem>();
        _renderer = GetComponentInChildren<SpriteRenderer>();

        _renderer.enabled = false;
    }

    /// <summary>Particle���Ăяo��</summary>
    private void PlayParticle()
    {
        _particleSystem.Play();
    }

    /// <summary>���C�g��L���ɂ���</summary>
    private void LightEnable()
    {
        _renderer.enabled = true;
    }

    /// <summary>���C�g�𖳌��ɂ���</summary>
    private void LightDisable()
    {
        _renderer.enabled = false;
    }
}
