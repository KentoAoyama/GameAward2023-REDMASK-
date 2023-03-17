using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UnityEngine.Events;

/// <summary>
/// �G�̊e�@�\�̒�����s���N���X
/// </summary>
public class EnemyActor : MonoBehaviour
{
    [SerializeField] private TransitionTimer _transitionTimer;
    [SerializeField] private EnemyStateMachine _enemyStateMachine;
    [SerializeField] private SightSensor _sightSensor;
    [SerializeField] private MoveBehavior _moveBehavior;
    [SerializeField] private AttackBehavior _attackBehavior;
    [SerializeField] private DefeatedBehavior _defeatedBehavior;

    [Header("���݂̃X�e�[�g��\�����邽�߂̃f�o�b�O�pUI")]
    [SerializeField] private Text _text;
    
    private Transform _player;
    //private ReactiveProperty<bool> _isPlayerDetected = new();
    private StateTransitionMessenger _stateTransitionMessenger;

    private void Awake()
    {
        _stateTransitionMessenger = new StateTransitionMessenger(gameObject.GetInstanceID());
    }

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        SubscribeTransitionWithTimeElapsed();
        //SubscribePlayerDetected();

        // �v���C���[�Ɍ����Ĉړ�����
        OnMessageReceived(BehaviorType.MoveToPlayer, () => _moveBehavior.StartRunToTarget(_player));
        // ���낤�낷��
        OnMessageReceived(BehaviorType.SearchMove, _moveBehavior.StartWalkToWanderingTarget);
        // �ړ����L�����Z������
        OnMessageReceived(BehaviorType.StopMove, _moveBehavior.CancelMoving);
        // �U��������
        OnMessageReceived(BehaviorType.Attack, _attackBehavior.Attack);
        // ���j�����
        OnMessageReceived(BehaviorType.Defeated, _defeatedBehavior.Defeated);
    }

    private void Update()
    {
        // TODO:���t���[���̌Ăяo���͕��ׂ�������̂ŌĂяo�����o��ݒ肷��
        //_isPlayerDetected.Value = _sightSensor.IsDetected();
        if (_sightSensor.IsDetected())
        {
            _stateTransitionMessenger.SendMessage(StateTransitionTrigger.PlayerFind);
        }
        else
        {
            _stateTransitionMessenger.SendMessage(StateTransitionTrigger.PlayerHide);
        }

        // �f�o�b�O�p�AUI�Ɍ��݂̃X�e�[�g��\������
        _text.text = _enemyStateMachine.CurrentState.Value.ToString();
    }

    /// <summary>
    /// �e�X�e�[�g����@�\�̌Ăяo���̃��b�Z�[�W����M�����ۂɎ��s���鏈����o�^����
    /// </summary>
    private void OnMessageReceived(BehaviorType type, UnityAction action)
    {
        MessageBroker.Default.Receive<BehaviorMessage>()
            .Where(message => message.ID == gameObject.GetInstanceID())
            .Where(message => message.Type == type)
            .Subscribe(_ => action.Invoke())
            .AddTo(this);
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
    //private void SubscribePlayerDetected()
    //{
    //    _isPlayerDetected.Subscribe(isPlayerDetected =>
    //    {
    //        if (isPlayerDetected)
    //        {
    //            _stateTransitionMessenger.SendMessage(StateTransitionTrigger.PlayerFind);
    //        }
    //        else
    //        {
    //            _stateTransitionMessenger.SendMessage(StateTransitionTrigger.PlayerHide);
    //        }
    //    }).AddTo(this);
    //}
}
