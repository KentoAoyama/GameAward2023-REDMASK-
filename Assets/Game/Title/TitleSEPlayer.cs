using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleSEPlayer : MonoBehaviour
{
    public void PlaySubmitSE()
    {
        GameManager.Instance.AudioManager.PlaySE("CueSheet_Gun", "SE_Enter");
    }
}
