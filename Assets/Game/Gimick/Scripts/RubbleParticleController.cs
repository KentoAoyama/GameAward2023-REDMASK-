using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubbleParticleController : MonoBehaviour, IDamageable
{
    [SerializeField]
    private GameObject _breakEffect;

    private ParticleSystem _particleSystem;
    private SpriteRenderer _renderer;

    private void Awake()
    {
        _particleSystem = GetComponent<ParticleSystem>();
        _renderer = GetComponent<SpriteRenderer>();
        _renderer.enabled = false;
    }

    void IDamageable.Damage()
    {
        _particleSystem.Play();
    }

    public void RubblePlay()
    {
        _particleSystem.Play();
        _renderer.enabled = true;
        GameManager.Instance.AudioManager.PlaySE("CueSheet_Gun", "SE_Gimmick_BrokenCeiling");
    }

    private void OnParticleCollision(GameObject other)
    {
        var eventList = new List<ParticleCollisionEvent>();
        var num = _particleSystem.GetCollisionEvents(other, eventList);

        for (int i = 0; i < num; i++)
        {
            var temp = Instantiate(_breakEffect);
            temp.transform.position = eventList[i].intersection;
            temp.GetComponent<ParticleSystem>().Play();
            Destroy(temp, 2F);
        }
    }
}
