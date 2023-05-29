using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainStopper : MonoBehaviour
{
    [SerializeField]
    private GameObject _rain;

    private void OnTriggerEnter(Collider other)
    {
        if ((other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Enemy")))
        {
            _rain.SetActive(false);
        }
    }
}
