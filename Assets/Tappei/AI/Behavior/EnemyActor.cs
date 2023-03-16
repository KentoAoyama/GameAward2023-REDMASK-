using UnityEngine;
using UnityEngine.UI;
using UniRx;

/// <summary>
/// �G�̊e�@�\�̒�����s���N���X
/// </summary>
public class EnemyActor : MonoBehaviour
{
    [SerializeField] private TransitionTimer _transitionTimer;
    [SerializeField] private EnemyStateMachine _enemyStateMachine;
    [SerializeField] private SightSensor _sightSensor;
    [SerializeField] private MoveController _moveController;

    [Header("���݂̃X�e�[�g��\�����邽�߂̃f�o�b�O�pUI")]
    [SerializeField] private Text _text;

    private Transform _player;
    private ReactiveProperty<bool> _isPlayerDetected = new();
    private StateTransitionMessenger _stateTransitionMessenger;

    private void Awake()
    {
        _stateTransitionMessenger = new StateTransitionMessenger(gameObject.GetInstanceID());
    }

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        SubscribeTransitionWithTimeElapsed();
        SubscribePlayerDetected();

        //MessageBroker.Default.Receive<BehaviorMessage>()
        //    .Where(message => message.ID == gameObject.GetInstanceID())
        //    .Where(message => message.Type == BehaviorType.Defeated)
        //    .Subscribe(_ => Defeated()).AddTo(this);

        // �v���C���[���������ۂ̃��b�Z�[�W����M
        // �v���C���[�Ɍ����Ĉړ�����
        MessageBroker.Default.Receive<BehaviorMessage>()
            .Where(message => message.ID == gameObject.GetInstanceID())
            .Where(message => message.Type == BehaviorType.MoveToPlayer)
            .Subscribe(_ => _moveController.StartRunToTarget(_player))
            .AddTo(this);

        //MessageBroker.Default.Receive<BehaviorMessage>()
        //    .Where(message => message.ID == gameObject.GetInstanceID())
        //    .Where(message => message.Type == BehaviorType.Attack)
        //    .Subscribe(_ => Attack()).AddTo(this);

        MessageBroker.Default.Receive<BehaviorMessage>()
            .Where(message => message.ID == gameObject.GetInstanceID())
            .Where(message => message.Type == BehaviorType.SearchMove)
            .Subscribe(_ =>
            {
                // �e�X�g�p
                _moveController.SearchMove();
            }).AddTo(this);

        MessageBroker.Default.Receive<BehaviorMessage>()
            .Where(message => message.ID == gameObject.GetInstanceID())
            .Where(message => message.Type == BehaviorType.StopMove)
            .Subscribe(_ =>
            {
                _moveController.CancelMoving();
            }).AddTo(this);
    }

    void Update()
    {
        // TODO:���t���[���̌Ăяo���͕��ׂ�������̂ŌĂяo�����o��ݒ肷��
        _isPlayerDetected.Value = _sightSensor.IsDetected();

        // �f�o�b�O�p�AUI�Ɍ��݂̃X�e�[�g��\������
        _text.text = _enemyStateMachine.CurrentState.Value.ToString();
    }

    /// <summary>
    /// Idle��Search�̃X�e�[�g�ɑJ�ڂ��Ă����ꍇ�͈�莞�Ԍ�Ɏ��Ԍo�߂ł̑J�ڂ̃��b�Z�[�W�𑗐M����
    /// ����ȊO�̃X�e�[�g�̏ꍇ�͎��Ԍo�߂̑J�ڂ��L�����Z������
    /// </summary>
    private void SubscribeTransitionWithTimeElapsed()
    {
        _enemyStateMachine.CurrentState.Subscribe(state =>
        {
            if(state.StateType == StateType.Idle || state.StateType == StateType.Search)
            {
                _transitionTimer.TimeElapsedExecute(() =>
                {
                    _stateTransitionMessenger.SendMessage(StateTransitionTrigger.TimeElapsed);
                });
            }
            else
            {
                _transitionTimer.ExecuteCancel();
            }


        }).AddTo(this);
    }

    /// <summary>
    /// �v���C���[�����E�ɑ�����/���E����������ۂɁA������g���K�[�Ƃ����J�ڂ̃��b�Z�[�W�𑗐M����
    /// </summary>
    private void SubscribePlayerDetected()
    {
        _isPlayerDetected.Subscribe(isPlayerDetected =>
        {
            if (isPlayerDetected)
            {
                _stateTransitionMessenger.SendMessage(StateTransitionTrigger.PlayerFind);
            }
            else
            {
                _stateTransitionMessenger.SendMessage(StateTransitionTrigger.PlayerHide);
            }
        }).AddTo(this);
    }
}
