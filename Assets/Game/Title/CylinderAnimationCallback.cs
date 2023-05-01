using UnityEngine;
using UnityEngine.Events;

public class CylinderAnimationCallback : MonoBehaviour
{
    [SerializeField]
    private UnityEvent _audioInAnimCallback;
    [SerializeField]
    private UnityEvent _audioOutAnimCallback;
    [SerializeField]
    private UnityEvent _manualInAnimCallback;
    [SerializeField]
    private UnityEvent _manualOutAnimCallback;
    [SerializeField]
    private UnityEvent _GallaryInAnimCallback;
    [SerializeField]
    private UnityEvent _GallaryOutAnimCallback;

    public void SylinderAnimCallback(CallbackType type)
    {
        if (type == CallbackType.AudioIn)
        {
            _audioInAnimCallback.Invoke();
        }
        else if (type == CallbackType.AudioOut)
        {
            _audioOutAnimCallback.Invoke();
        }
        else if (type == CallbackType.ManualIn)
        {
            _manualInAnimCallback.Invoke();
        }
        else if (type == CallbackType.ManualOut)
        {
            _manualOutAnimCallback.Invoke();
        }
        else if (type == CallbackType.GallaryIn)
        {
            _GallaryInAnimCallback.Invoke();
        }
        else
        {
            _GallaryOutAnimCallback.Invoke();
        }
    }
}

[System.Serializable]
public enum CallbackType
{
    AudioIn,
    AudioOut,
    ManualIn,
    ManualOut,
    GallaryIn,
    GallaryOut
}
