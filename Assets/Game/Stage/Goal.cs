// 日本語対応
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    [TagName, SerializeField]
    private string _playerTag = default;

    [SerializeField]
    private StageController _stageController = default;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == _playerTag)
        {
            _stageController.StageComplete();
        }
    }
}
