using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuWindowClosedButton : MonoBehaviour
{
    [SerializeField]
    private GameObject _nextSelectButton = default;

    private void OnDisable()
    {
        EventSystem.current.SetSelectedGameObject(_nextSelectButton.gameObject);
    }
}
