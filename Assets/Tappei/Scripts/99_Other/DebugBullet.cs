using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugBullet : MonoBehaviour
{
    [SerializeField] Rigidbody2D _rb;
    [SerializeField] Vector3 _dir;

    void Start()
    {
        _rb.AddForce(_dir.normalized * 350);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(gameObject);
    }
}
