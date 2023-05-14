// 日本語対応
using UnityEngine;

public class AnimPauseResume : MonoBehaviour, IPausable
{
    [AnimationParameter, SerializeField]
    private string _animationSpeedParamName = default;
    private Animator _animator = default;
    private float _normalSpeed = default;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _normalSpeed = _animator.GetFloat(_animationSpeedParamName);
    }
    private void OnEnable()
    {
        GameManager.Instance.PauseManager.Register(this);
    }
    private void OnDisable()
    {
        GameManager.Instance.PauseManager.Lift(this);
    }

    public void Pause()
    {
        _animator.SetFloat(_animationSpeedParamName, 0f);
    }

    public void Resume()
    {
        _animator.SetFloat(_animationSpeedParamName, _normalSpeed);
    }
}
