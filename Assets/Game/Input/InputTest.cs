// 日本語対応
using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputTest : MonoBehaviour
{
    InputManager _inputManager = new();

    private void Start()
    {
        _inputManager.Init();
    }

    private void Update()
    {
        if (_inputManager.IsPressed[InputType.MoveHorizontal])
            Debug.Log("移動入力が発生したよ");
        if (_inputManager.IsExist[InputType.MoveHorizontal])
            Debug.LogWarning("移動入力があるよ");
        if (_inputManager.IsReleased[InputType.MoveHorizontal])
            Debug.LogError("移動入力がなくなったよ");
    }
}
