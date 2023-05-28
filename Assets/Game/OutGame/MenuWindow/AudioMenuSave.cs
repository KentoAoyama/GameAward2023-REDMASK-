using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioMenuSave : MonoBehaviour
{
    private void OnDisable()
    {
        GameManager.Instance.AudioManager.Save();
    }
}
