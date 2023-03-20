using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// ���̃N���X��p���Ď��Ԍo�߂ɂ��X�e�[�g�̑J�ڂ��s��
/// ���
/// </summary>
public class TransitionTimer
{

    //[Header("�X�e�[�g�̑J�ڂ܂ł̒x������(�b)")]
    //[SerializeField] private float _minDelay = 2.0f;
    //[SerializeField] private float _maxDelay = 3.0f;

    // TODO:���̃^�C���X�s�[�h�̒l�ɂȂ��Ă���
    private float _timeSpeed = 1;

    private CancellationTokenSource _cts;

    private void OnDisable()
    {
        _cts?.Cancel();
    }

    /// <summary>
    /// �O������Ăяo���ہA�R�[���o�b�N�ɑJ�ڏ�����n�����Ƃň�莞�Ԍ�̑J�ڂ��s��
    /// ���̃��\�b�h�̎��s���ɍēx�Ăяo���ƈȑO�̏����̓L�����Z�������
    /// </summary>
    public void TimeElapsedExecute(UnityAction callback)
    {
        ExecuteCancel();
        _cts = new();

        //float delayTime = Random.Range(_minDelay, _maxDelay);
        //TimerAsync(callback, delayTime).Forget();
    }

    /// <summary>
    /// Update()�̃^�C�~���O�Ń^�C���X�s�[�h�ɉ������l�𑫂����Ƃ�
    /// �X���[���[�V������|�[�Y�ɑΉ������Ă���
    /// </summary>
    private async UniTaskVoid TimerAsync(UnityAction callback, float delayTime)
    {
        _cts.Token.ThrowIfCancellationRequested();
        
        float total = 0;
        while (total <= delayTime)
        {
            total += _timeSpeed * Time.deltaTime;
            await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken: _cts.Token);
        }

        callback?.Invoke();
    }

    public void ExecuteCancel() => _cts?.Cancel();
}
