using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CeilingParticleCalls : MonoBehaviour
{
    private void OnParticleCollision(GameObject other)
    {
        other.gameObject.TryGetComponent<IDamageable>(out IDamageable damageable);
        damageable?.Damage();
    }

}
