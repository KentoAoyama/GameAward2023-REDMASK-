using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugLineSender : MonoBehaviour
{
    public void SendLine()
    {
        string[] array =
        {
            "あああ",
            "いいい",
            "ううう",
            "えええ",
            "おおお",
            "ああああ",
            "いいいい",
            "うううう",
            "ええええ",
            "おおおお",
            "あああああ",
            "いいいいい",
            "ううううう",
            "えええええ",
            "おおおおお",
        };

        int r = Random.Range(0, array.Length);

        LineMessageSender.SendMessage(array[r]);
    }
}
