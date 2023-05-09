using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubbleParticleController : MonoBehaviour
{
    private ParticleSystem _particleSystem;

    private void Awake()
    {
        _particleSystem = GetComponent<ParticleSystem>();
    }

    private void OnParticleCollision(GameObject other)
    {
        var eventList = new List<ParticleCollisionEvent>();
        _particleSystem.GetCollisionEvents(other, eventList);
        
        foreach (ParticleCollisionEvent particleCollisionEvent in eventList)
        {
        }
    }
}
