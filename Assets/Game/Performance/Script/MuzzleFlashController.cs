using UnityEngine;
using UnityEngine.Rendering.Universal;

public class MuzzleFlashController : MonoBehaviour
{
    /// <summary>アニメーションさせるParticle</summary>
    private ParticleSystem _particleSystem;
    /// <summary>アニメーションさせるLight2D</summary>
    private Light2D _light2D;

    private void Awake()
    {
        _particleSystem = GetComponentInChildren<ParticleSystem>();
        _light2D = GetComponentInChildren<Light2D>();

        //_light2D.enabled = false;
    }

    /// <summary>Particleを呼び出す</summary>
    private void PlayParticle()
    {
        _particleSystem.Play();
    }

    /// <summary>ライトを有効にする</summary>
    private void LightEnable()
    {
       // _light2D.enabled = true;
    }

    /// <summary>ライトを無効にする</summary>
    private void LightDisable()
    {
        //_light2D.enabled = false;
    }
}
