using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Command : MonoBehaviour
{
    // 1122343456
    Queue<int> _q = new();

    int[] _a = new int[] { 1, 1, 2, 2, 3, 4, 3, 4, 5, 6 };

    void Update()
    {
        if (Gamepad.current == null) return;

        if (Gamepad.current.dpad.up.wasPressedThisFrame)
        {
            _q.Enqueue(1);
        }
        else if (Gamepad.current.dpad.down.wasPressedThisFrame)
        {
            _q.Enqueue(2);
        }
        else if (Gamepad.current.dpad.left.wasPressedThisFrame)
        {
            _q.Enqueue(3);
        }
        else if (Gamepad.current.dpad.right.wasPressedThisFrame)
        {
            _q.Enqueue(4);
        }
        else if (Gamepad.current.buttonSouth.wasPressedThisFrame)
        {
            _q.Enqueue(5);
        }
        else if (Gamepad.current.buttonEast.wasPressedThisFrame)
        {
            _q.Enqueue(6);
        }
        else if (Gamepad.current.buttonNorth.wasPressedThisFrame)
        {
            _q.Clear();
        }

        if (_q.Count == 10)
        {
            bool ok = true;
            for(int i = 0; i < 10; i++)
            {
                int n = _q.Dequeue();
                int m = _a[i];

                if (n != m) ok = false;
            }

            if (ok)
            {
                D();
            }
        }
    }

    void D()
    {
        foreach(var v in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            v.GetComponent<EnemyController>().Jump();
        }
    }
}
