using UnityEngine;
using UnityEngine.UI;
using UniRx;

/// <summary>
/// 敵の各機能の仲介を行うクラス
/// </summary>
public class EnemyActor : MonoBehaviour
{
    [SerializeField] private TransitionWithTimeElapsed _transitionWithTimeElapsed;
    [SerializeField] private EnemyStateMachine _enemyStateMachine;
    [SerializeField] private SightSensor _sightSensor;

    [Header("現在のステートを表示するためのデバッグ用UI")]
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

    // デバッグ系のうｐだて
    void Update()
    {
        _text.text = _enemyStateMachine.CurrentState.Value.ToString();
        Debug.Log(_sightSensor.IsDetected());
    }

    // アイドル/うろうろ状態の時は時間経過でステートを遷移する必要がある
    // ステート側でタイマーの起動をすることもできるが、そうするとステート側に遷移の条件を持たせるのと同じ
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
