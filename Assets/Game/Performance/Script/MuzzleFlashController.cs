using UnityEngine;
using UnityEngine.Rendering.Universal;

public class MuzzleFlashController : MonoBehaviour
{
    /// <summary>�A�j���[�V����������Particle</summary>
    private ParticleSystem _particleSystem;
    /// <summary>�A�j���[�V����������Light2D</summary>
    private Light2D _light2D;

    private void Awake()
    {
        _particleSystem = GetComponentInChildren<ParticleSystem>();
        _light2D = GetComponentInChildren<Light2D>();

        //_light2D.enabled = false;
    }

    /// <summary>Particle���Ăяo��</summary>
    private void PlayParticle()
    {
        _particleSystem.Play();
    }

    /// <summary>���C�g��L���ɂ���</summary>
    private void LightEnable()
    {
       // _light2D.enabled = true;
    }

    /// <summary>���C�g�𖳌��ɂ���</summary>
    private void LightDisable()
    {
        //_light2D.enabled = false;
    }
}
