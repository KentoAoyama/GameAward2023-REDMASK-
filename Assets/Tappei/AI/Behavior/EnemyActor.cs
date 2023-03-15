using UnityEngine;
using UnityEngine.UI;
using UniRx;

/// <summary>
/// �G�̊e�@�\�̒�����s���N���X
/// </summary>
public class EnemyActor : MonoBehaviour
{
    [SerializeField] private TransitionWithTimeElapsed _transitionWithTimeElapsed;
    [SerializeField] private EnemyStateMachine _enemyStateMachine;
    [SerializeField] private SightSensor _sightSensor;

    [Header("���݂̃X�e�[�g��\�����邽�߂̃f�o�b�O�pUI")]
    [SerializeField] private Text _text;

    private StateTransitionMessenger _stateTransitionMessageSender;

    private void Awake()
    {
        _stateTransitionMessageSender = new StateTransitionMessenger(gameObject.GetInstanceID());
    }

    private void Start()
    {
        SubscribeTransitionWithTimeElapsed();
    }

    // �f�o�b�O�n�̂�������
    void Update()
    {
        _text.text = _enemyStateMachine.CurrentState.Value.ToString();
        Debug.Log(_sightSensor.IsDetected());
    }

    // �A�C�h��/���낤���Ԃ̎��͎��Ԍo�߂ŃX�e�[�g��J�ڂ���K�v������
    // �X�e�[�g���Ń^�C�}�[�̋N�������邱�Ƃ��ł��邪�A��������ƃX�e�[�g���ɑJ�ڂ̏�������������̂Ɠ���
    private void SubscribeTransitionWithTimeElapsed()
    {
        _enemyStateMachine.CurrentState
            .Where(state => state.StateType == StateType.Idle || state.StateType == StateType.Search)
            .Subscribe(_ =>
            {
                _transitionWithTimeElapsed.DelayedSendTransitionMessage(_stateTransitionMessageSender);
            }).AddTo(this);
    }
}
