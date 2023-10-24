using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyStates 
{
    public abstract void EnterState(Enemy enemy);

    public abstract void UpdateState(Enemy enemy);

    public abstract void ExitState(Enemy enemy);
}
