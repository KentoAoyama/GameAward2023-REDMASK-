using System;
using UnityEngine;

public class BulletCollisonSparkController : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem[] _sparks = default;

    public void PlaySpark()
    {
        Array.ForEach(_sparks, x => x.Play());
    }
}
