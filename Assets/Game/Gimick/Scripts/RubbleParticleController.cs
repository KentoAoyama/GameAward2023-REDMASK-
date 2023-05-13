using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubbleParticleController : MonoBehaviour
{
    [SerializeField]
    private GameObject _breakEffect;

    private ParticleSystem _particleSystem;

    private void Awake()
    {
        _particleSystem = GetComponent<ParticleSystem>();
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
