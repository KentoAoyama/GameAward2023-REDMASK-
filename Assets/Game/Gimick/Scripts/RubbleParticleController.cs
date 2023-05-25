using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubbleParticleController : MonoBehaviour, IDamageable
{
    [SerializeField]
    private GameObject _breakEffect;

    private ParticleSystem _particleSystem;
    [SerializeField]
    private SpriteRenderer _renderer;
    [SerializeField]
    private Sprite _holeSprite;

    private void Awake()
    {
        _particleSystem = GetComponent<ParticleSystem>();
    }

    void IDamageable.Damage()
    {
        RubblePlay();
    }

    public void RubblePlay()
    {
        _renderer.sprite = _holeSprite;
        _particleSystem.Play();
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

        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<IDamageable>().Damage();
        }

        Debug.Log(other.name);
    }
}
