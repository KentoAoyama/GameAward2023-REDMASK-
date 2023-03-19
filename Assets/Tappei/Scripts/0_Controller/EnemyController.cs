using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UnityEngine.Events;


/// <summary>
/// 各振る舞いのクラスのメソッドを組み合わせて行動を制御するクラス
/// </summary>
[RequireComponent(typeof(SightSensor))]
[RequireComponent(typeof(MoveBehavior))]
[RequireComponent(typeof(AttackBehavior))]
[RequireComponent(typeof(DefeatedBehavior))]
public class EnemyController : MonoBehaviour
{
    [Header("敵の各種パラメーターを設定したSO")]
    [Tooltip("各振る舞いのクラスはこのSO内の値を参照して機能する")]
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

    // ポーズ/再開機能が必要

    // 各ステートが各振る舞いにアクセスするためのFacadeが欲しい
    // Facade.Attack();でAttackBehaviorのattackが呼ばれるイメージ
    // 遷移もなるべくきれいに書きたい
    // 戻り値が欲しい

    [Header("攻撃範囲")]
    [SerializeField] private float _attackRane = 3.0f;
    [Header("現在のステートを表示するためのデバッグ用UI")]
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

        //// プレイヤーに向けて移動する
        //OnMessageReceived(BehaviorType.MoveToPlayer, () => _moveBehavior.StartRunToTarget(_player));
        //// うろうろする
        //OnMessageReceived(BehaviorType.SearchMove, _moveBehavior.StartWalkToWanderingTarget);
        //// 移動をキャンセルする
        //OnMessageReceived(BehaviorType.StopMove, _moveBehavior.CancelMoving);
        //// 攻撃をする
        //OnMessageReceived(BehaviorType.Attack, _attackBehavior.Attack);
        //// 撃破される
        //OnMessageReceived(BehaviorType.Defeated, _defeatedBehavior.Defeated);
    }

    private void Update()
    {
        //float distance = _sightSensor.TryGetDistanceToPlayer();

        // TODO:毎フレームの呼び出しは負荷がかかるので呼び出し感覚を設定する
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

        // デバッグ用、UIに現在のステートを表示する
        //_text.text = _enemyStateMachine.CurrentState.Value.ToString();
    }

    private void OnDisable()
    {
        // この敵が破棄される際の処理
    }

    /// <summary>
    /// 各ステートから機能の呼び出しのメッセージを受信した際に実行する処理を登録する
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
    /// IdleとSearchのステートに遷移してきた場合は一定時間後に時間経過での遷移のメッセージを送信する
    /// それ以外のステートの場合は時間経過の遷移をキャンセルする
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
