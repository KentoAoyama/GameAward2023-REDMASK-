using UnityEngine;

public class BehaviorFacade
{


    public BehaviorFacade(GameObject gameObject, EnemyParamsSO enemyParamsSO)
    {

        Params = enemyParamsSO;
    }

    public EnemyParamsSO Params { get; }
}
