using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UnityEngine.Events;


/// <summary>
/// �e�U�镑���̃N���X�̃��\�b�h��g�ݍ��킹�čs���𐧌䂷��N���X
/// </summary>
[RequireComponent(typeof(SightSensor))]
[RequireComponent(typeof(MoveBehavior))]
[RequireComponent(typeof(AttackBehavior))]
[RequireComponent(typeof(DefeatedBehavior))]
public class EnemyController : MonoBehaviour
{
    [Header("�G�̊e��p�����[�^�[��ݒ肵��SO")]
    [Tooltip("�e�U�镑���̃N���X�͂���SO���̒l���Q�Ƃ��ċ@�\����")]
    [SerializeField] private EnemyParamsSO _enemyParamsSO;

    private ReactiveProperty<StateTypeBase> _currentState = new();
    private StateRegister _stateRegister;
    private BehaviorFacade _behaviorFacade;

    private void Awake()
    {
        InitCreateInstance();
        InitStateRegister();
        InitCurrentState();
    }

    private void InitCreateInstance()
    {
        _behaviorFacade = new(gameObject, _enemyParamsSO);
        _stateRegister = new();
    }

    private void InitStateRegister()
    {
        _stateRegister.Register(StateType.Idle, _behaviorFacade);
        _stateRegister.Register(StateType.Search, _behaviorFacade);
        _stateRegister.Register(StateType.Move, _behaviorFacade);
        _stateRegister.Register(StateType.Attack, _behaviorFacade);
        _stateRegister.Register(StateType.Defeated, _behaviorFacade);
    }

    private void InitCurrentState()
    {
        StateType state = _enemyParamsSO.EntryState;
        _currentState.Value = _stateRegister.GetState(state);
    }

    // �|�[�Y/�ĊJ�@�\���K�v

    // �e�X�e�[�g���e�U�镑���ɃA�N�Z�X���邽�߂�Facade���~����
    // Facade.Attack();��AttackBehavior��attack���Ă΂��C���[�W
    // �J�ڂ��Ȃ�ׂ����ꂢ�ɏ�������
    // �߂�l���~����

    [Header("�U���͈�")]
    [SerializeField] private float _attackRane = 3.0f;
    [Header("���݂̃X�e�[�g��\�����邽�߂̃f�o�b�O�pUI")]
    [SerializeField] private Text _text;
    
    private Transform _player;
    //private ReactiveProperty<bool> _isPlayerDetected = new();
    private StateTransitionMessenger _stateTransitionMessenger;

    //private void Awake()
    //{
    //    _stateTransitionMessenger = new StateTransitionMessenger(gameObject.GetInstanceID());
    //}

    private void Start()
    {
        //_player = GameObject.FindGameObjectWithTag("Player").transform;
        //SubscribeTransitionWithTimeElapsed();
        ////SubscribePlayerDetected();

        //// �v���C���[�Ɍ����Ĉړ�����
        //OnMessageReceived(BehaviorType.MoveToPlayer, () => _moveBehavior.StartRunToTarget(_player));
        //// ���낤�낷��
        //OnMessageReceived(BehaviorType.SearchMove, _moveBehavior.StartWalkToWanderingTarget);
        //// �ړ����L�����Z������
        //OnMessageReceived(BehaviorType.StopMove, _moveBehavior.CancelMoving);
        //// �U��������
        //OnMessageReceived(BehaviorType.Attack, _attackBehavior.Attack);
        //// ���j�����
        //OnMessageReceived(BehaviorType.Defeated, _defeatedBehavior.Defeated);
    }

    private void Update()
    {
        //float distance = _sightSensor.TryGetDistanceToPlayer();

        // TODO:���t���[���̌Ăяo���͕��ׂ�������̂ŌĂяo�����o��ݒ肷��
        //_isPlayerDetected.Value = _sightSensor.IsDetected();

        //if (Mathf.Approximately(distance, -1))
        //{
        //    _stateTransitionMessenger.SendMessage(StateTransitionTrigger.PlayerHide);
        //}
        //else if (distance <= _attackRane)
        //{
        //    _stateTransitionMessenger.SendMessage(StateTransitionTrigger.PlayerInAttackRange);
        //}
        //else if (distance > _attackRane)
        //{
        //    _stateTransitionMessenger.SendMessage(StateTransitionTrigger.PlayerFind);
        //}

        // �f�o�b�O�p�AUI�Ɍ��݂̃X�e�[�g��\������
        //_text.text = _enemyStateMachine.CurrentState.Value.ToString();
    }

    private void OnDisable()
    {
        // ���̓G���j�������ۂ̏���
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
        //_enemyStateMachine.CurrentState.Subscribe(state =>
        //{
        //    if(state.StateType == StateType.Idle || state.StateType == StateType.Search)
        //    {
        //        _transitionTimer.TimeElapsedExecute(() =>
        //        {
        //            _stateTransitionMessenger.SendMessage(StateTransitionTrigger.TimeElapsed);
        //        });
        //    }
        //    else
        //    {
        //        _transitionTimer.ExecuteCancel();
        //    }
        //}).AddTo(this);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if (collision.gameObject.CompareTag("EditorOnly"))
        //{
        //    _defeatedBehavior.Defeated();
        //}
    }
}
