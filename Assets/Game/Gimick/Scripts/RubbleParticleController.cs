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

    private void OnCollisionEnter2D(Collision2D other)
    {
        foreach (ContactPoint2D contactPoint in other.contacts)
        {
            Debug.Log(contactPoint.point.ToString());
        }
    }
}
