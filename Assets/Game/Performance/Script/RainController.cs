using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainController : MonoBehaviour
{
    private int _rainIndex = -1;
    private void OnEnable()
    {
        _rainIndex = GameManager.Instance.AudioManager.PlaySE("CueSheet_Gun", "SE_Rain");
    }

    private void OnDisable()
    {
        GameManager.Instance.AudioManager.StopSE(_rainIndex);
    }
}
