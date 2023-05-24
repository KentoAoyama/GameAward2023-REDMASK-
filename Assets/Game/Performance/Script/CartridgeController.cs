using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartridgeController : MonoBehaviour
{
    private ParticleSystem _particleSystem = default;
    private ParticleSystem.MainModule _mainModule = default;
    private ParticleSystem.EmissionModule _emissonModule = default;
    private List<ParticleCollisionEvent> _events = new List<ParticleCollisionEvent>();
    private int _cartridgeCount = 0;

    private void Awake()
    {
            _particleSystem = GetComponent<ParticleSystem>();
            _mainModule = _particleSystem.main;
            _emissonModule = _particleSystem.emission;
    }

    /// <summary>薬莢を落とすエフェクトを再生する</summary>
    /// <param name="count">薬莢の数</param>
    public void CartridgePlay(int count)
    {
        if (count < 1) return;

        _particleSystem.Stop();
        _emissonModule = _particleSystem.emission;

        float time = (_mainModule.duration / count);

        var temp = new ParticleSystem.Burst(0, 1, 1, count, time);
        _emissonModule.SetBurst(0, temp);

        _cartridgeCount = count;
        _particleSystem.Play();
        Debug.Log("Play");
    }

    private void OnParticleCollision(GameObject other)
    {
        var collCount = _particleSystem.GetCollisionEvents(other, _events);

        if (collCount > 0 && _cartridgeCount > 0)
        {
            if (_cartridgeCount <= 1)
            {
                GameManager.Instance.AudioManager.PlaySE("CueSheet_Gun", "SE_Bullet_Drop1");
            }
            else if (_cartridgeCount <= 2)
            {
                GameManager.Instance.AudioManager.PlaySE("CueSheet_Gun", "SE_Bullet_Drop2");
            }
            else
            {
                GameManager.Instance.AudioManager.PlaySE("CueSheet_Gun", "SE_Bullet_Drop3over");
            }

            _cartridgeCount = 0;
        }
    }
}
