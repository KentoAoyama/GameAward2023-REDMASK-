using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestPause : MonoBehaviour
{

    void Update()
    {
        if(Keyboard.current.gKey.wasPressedThisFrame)
        {
            GameManager.Instance.PauseManager.ExecutePause();
        }

        if (Keyboard.current.hKey.wasPressedThisFrame)
        {
            GameManager.Instance.PauseManager.ExecuteResume();
        }
    }
}
