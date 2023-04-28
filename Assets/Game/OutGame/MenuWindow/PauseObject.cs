// 日本語対応
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseObject : MonoBehaviour
{
    private void OnEnable()
    {
        GameManager.Instance.PauseManager.ExecutePause();
    }
    private void OnDisable()
    {
        GameManager.Instance.PauseManager.ExecuteResume();
    }
}
