using UnityEngine;
using UnityEngine.Rendering.Universal;

public class MuzzleFlashController : MonoBehaviour
{
    /// <summary>アニメーションさせるParticle</summary>
    private ParticleSystem _particleSystem;
    /// <summary>アニメーションさせるLight2D</summary>
    private SpriteRenderer _renderer;

    private void Awake()
    {
        _particleSystem = GetComponentInChildren<ParticleSystem>();
        _renderer = GetComponentInChildren<SpriteRenderer>();

        _renderer.enabled = false;
    }

    /// <summary>Particleを呼び出す</summary>
    private void PlayParticle()
    {
        _particleSystem.Play();
    }

    /// <summary>ライトを有効にする</summary>
    private void LightEnable()
    {
        _renderer.enabled = true;
    }

    /// <summary>ライトを無効にする</summary>
    private void LightDisable()
    {
        _renderer.enabled = false;
    }
}
