using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer _doorRenderer = default;
    [SerializeField]
    private Sprite _openSprite = default;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.gameObject.tag);
        if(other.gameObject.CompareTag("Player"))
        {
            _doorRenderer.sprite = _openSprite;
        }    
    }
}
