using UnityEngine;
using UnityEngine.Rendering.Universal;
using DG.Tweening;

public class MuzzleFlashController : MonoBehaviour
{
    [SerializeField, Tooltip("アニメーションの時間(秒)")]
    private float _animDuration = 0.5f;
    /// <summary>�A�j���[�V����������Particle</summary>
    private ParticleSystem _particleSystem = default;
    /// <summary>�A�j���[�V����������Light2D</summary>
    private Light2D _muzzleFlash = default;

    private Sequence _currentAnimation;

    private void Awake()
    {
        _particleSystem = GetComponentInChildren<ParticleSystem>();
        _muzzleFlash = GetComponentInChildren<Light2D>();
        _muzzleFlash.enabled = false;
    }

    public void PlayMuzzleFlash()
    {
        if(_currentAnimation != null)
        {
            _currentAnimation.Kill();
        }

        _currentAnimation = DOTween.Sequence();

        _currentAnimation.OnStart(() => 
            {
                _particleSystem.Play();
                _muzzleFlash.enabled = true; 
            })
            .SetDelay(_animDuration)
            .OnComplete(() => 
            {
                _muzzleFlash.enabled = false;
                _currentAnimation = null;
            })
            .OnKill(() =>
            {
                _muzzleFlash.enabled = false;
                _currentAnimation = null;
            });
            
    }
}
