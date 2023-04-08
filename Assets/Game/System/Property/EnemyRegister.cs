// 日本語対応
using System.Collections.Generic;

public class EnemyRegister
{
    private HashSet<EnemyTypeBehavior> _enemies = new HashSet<EnemyTypeBehavior>();

    public IReadOnlyCollection<EnemyTypeBehavior> Enemies => _enemies;

    public void Register(EnemyTypeBehavior enemy)
    {
        _enemies.Add(enemy);
    }
    public void Lift(EnemyTypeBehavior enemy)
    {
        _enemies.Remove(enemy);
    }
}
