using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultController : MonoBehaviour
{
    [SerializeField]
    private int _cutImageNum = 6;

    private void OnDisable()
    {
        GameManager.Instance.GalleryManager.SetOpenedID(true, _cutImageNum);
    }
}
