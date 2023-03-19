using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class BehaviorFacade
{
    private SightSensor _sightSensor;
    private MoveBehavior _moveBehavior;
    private AttackBehavior _attackBehavior;
    private DefeatedBehavior _defeatedBehavior;

    public BehaviorFacade(GameObject gameObject, EnemyParamsSO enemyParamsSO)
    {
        _sightSensor = gameObject.GetComponent<SightSensor>();
        _moveBehavior = gameObject.GetComponent<MoveBehavior>();
        _attackBehavior = gameObject.GetComponent<AttackBehavior>();
        _defeatedBehavior = gameObject.GetComponent<DefeatedBehavior>();
        Params = enemyParamsSO;
    }

    public EnemyParamsSO Params { get; }

    UnityAction OnAttack;
    UnityAction OnDefeated;
}
