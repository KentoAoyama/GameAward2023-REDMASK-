using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugLineSender : MonoBehaviour
{
    public void SendLine()
    {
        string[] array =
        {
            "‚ ‚ ‚ ",
            "‚¢‚¢‚¢",
            "‚¤‚¤‚¤",
            "‚¦‚¦‚¦",
            "‚¨‚¨‚¨",
            "‚ ‚ ‚ ‚ ",
            "‚¢‚¢‚¢‚¢",
            "‚¤‚¤‚¤‚¤",
            "‚¦‚¦‚¦‚¦",
            "‚¨‚¨‚¨‚¨",
            "‚ ‚ ‚ ‚ ‚ ",
            "‚¢‚¢‚¢‚¢‚¢",
            "‚¤‚¤‚¤‚¤‚¤",
            "‚¦‚¦‚¦‚¦‚¦",
            "‚¨‚¨‚¨‚¨‚¨",
        };

        int r = Random.Range(0, array.Length);

        LineMessageSender.SendMessage(array[r]);
    }
}
