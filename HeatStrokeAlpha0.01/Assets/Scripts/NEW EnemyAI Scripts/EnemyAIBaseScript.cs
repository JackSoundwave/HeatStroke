using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*This abstract class serves as the base for the enemyStateManager. Basically the same as the PlayerUnitBaseState.cs*/
public abstract class EnemyAIBaseScript
{
    //lorem ipsum
    public abstract void EnterState(EnemyAIStateManager enemy);

    //me when I lorem the ipsum fr fr
    public abstract void UpdateState(EnemyAIStateManager enemy);
}
