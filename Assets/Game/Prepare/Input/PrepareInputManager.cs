// 日本語対応
using UnityEngine;

public class PrepareInputManager : MonoBehaviour
{
    private PrepareInputController _prepareInputController = null;

    public PrepareInputController PrepareInputController => _prepareInputController;

    private void Awake()
    {
        _prepareInputController = new PrepareInputController();
        _prepareInputController.Enable();
    }
}