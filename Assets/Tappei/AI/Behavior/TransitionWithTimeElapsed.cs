using DG.Tweening;
using UnityEngine;

/// <summary>
/// ���̃N���X��p���Ď��Ԍo�߂ɂ��X�e�[�g�̑J�ڂ��s��
/// </summary>
public class TransitionWithTimeElapsed : MonoBehaviour
{
    [Header("�X�e�[�g�̑J�ڂ܂ł̒x������")]
    [SerializeField] private float _minDelay = 2.0f;
    [SerializeField] private float _maxDelay = 3.0f;

    private Tween _tween;

    private void OnDisable()
    {
        _tween?.Kill();
    }

    /// <summary>
    /// ��莞�Ԍ�Ɏ��Ԍo�߂őJ�ڂ������邽�߂̃��b�Z�[�W�𑗐M����
    /// ���Ԍo�߂őJ�ڂ���X�e�[�g�ɑJ�ڂ����ۂɌĂ΂��
    /// </summary>
    public void DelayedSendTransitionMessage(StateTransitionMessenger messageSender)
    {
        if (_tween != null)
        {
            _tween.Kill();
            Debug.LogWarning("�d�����ČĂ΂ꂽ�̂ňȑO�Ă΂ꂽ�������L�����Z�����܂�");
        }

        float delayTime = Random.Range(_minDelay, _maxDelay);
        _tween = DOVirtual.DelayedCall(delayTime, () =>
        {
            messageSender.SendMessage(StateTransitionTrigger.TimeElapsed);
        }, ignoreTimeScale: false);
    }

    // ��x�Ă΂ꂽ��X���[���[�V�����ƃ|�[�Y�ɑΉ����Ă��Ȃ�
    // �X���[�ɂȂ��Ă���Ԃ͑J�ڂ܂ł̃J�E���g���������Ȃ�
    // �|�[�Y�̏ꍇ�̓J�E���g��0�ɂȂ�
    // UniRx�ł�����H
    // ���\�b�h���Ă΂��
    // ���t���[������̒l�𑫂�����ł���
    // �X���[���[�V����or�|�[�Y�̏ꍇ�͂��̒l���ς��

    int count = 0;
    public void DelayedSend()
    {
        count += 1;
    }
}
