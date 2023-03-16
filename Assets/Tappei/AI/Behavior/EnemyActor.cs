using UnityEngine;
using UnityEngine.UI;
using UniRx;

/// <summary>
/// 敵の各機能の仲介を行うクラス
/// </summary>
public class EnemyActor : MonoBehaviour
{
    [SerializeField] private TransitionTimer _transitionTimer;
    [SerializeField] private EnemyStateMachine _enemyStateMachine;
    [SerializeField] private SightSensor _sightSensor;
    [SerializeField] private MoveController _moveController;

    [Header("現在のステートを表示するためのデバッグ用UI")]
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

        // プレイヤーを見つけた際のメッセージを受信
        // プレイヤーに向けて移動する
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
                // テスト用
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
        // TODO:毎フレームの呼び出しは負荷がかかるので呼び出し感覚を設定する
        _isPlayerDetected.Value = _sightSensor.IsDetected();

        // デバッグ用、UIに現在のステートを表示する
        _text.text = _enemyStateMachine.CurrentState.Value.ToString();
    }

    /// <summary>
    /// IdleとSearchのステートに遷移してきた場合は一定時間後に時間経過での遷移のメッセージを送信する
    /// それ以外のステートの場合は時間経過の遷移をキャンセルする
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
    /// プレイヤーを視界に捉えた/視界から消えた際に、それをトリガーとした遷移のメッセージを送信する
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
