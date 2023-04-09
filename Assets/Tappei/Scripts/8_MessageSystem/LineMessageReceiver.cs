using Cysharp.Threading.Tasks;
using System.Threading;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �䎌��\��������̂Ɏg�p����N���X
/// �󂯎�鑤
/// </summary>
public class LineMessageReceiver : MonoBehaviour
{
    [Header("�䎌��\������e�L�X�g")]
    [SerializeField] private Text _text;
    [Header("�\������䎌���X�V����Ԋu(�b)")]
    [SerializeField] private float _interval = 1.0f;

    private string _nextLine;

    private void Awake()
    {
        // ��ԍŌ�Ɏ�M�������b�Z�[�W�����̑䎌�ɔ��f�����
        MessageBroker.Default.Receive<LineMessage>().Subscribe(message =>
        {
            _nextLine = message.Line;

        }).AddTo(this);
    }

    private void Start()
    {
        LineControlAsync(this.GetCancellationTokenOnDestroy()).Forget();
    }

    /// <summary>
    /// �䎌����M������e�L�X�g�ɕ\�����Ă��̃e�L�X�g����\���ɂȂ�̂�҂Ƃ���������J��Ԃ�
    /// </summary>
    private async UniTaskVoid LineControlAsync(CancellationToken token)
    {
        while (true)
        {
            token.ThrowIfCancellationRequested();

            await UniTask.WaitUntil(() => _nextLine != string.Empty, cancellationToken: token);
            string line = _nextLine;
            _nextLine = string.Empty;
            await SetTextAsync(line, token);
        }
    }

    private async UniTask SetTextAsync(string line, CancellationToken token)
    {
        _text.text = line;
        await UniTask.Delay(System.TimeSpan.FromSeconds(_interval), cancellationToken: token);
        _text.text = string.Empty;
    }
}
