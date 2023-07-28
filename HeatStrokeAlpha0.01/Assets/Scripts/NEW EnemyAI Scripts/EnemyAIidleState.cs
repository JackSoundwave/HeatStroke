using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAIidleState : EnemyAIBaseScript
{
    
    public override void EnterState(EnemyAIStateManager enemy)
    {
        Debug.Log("Enemy is idle");
    }
    public override void UpdateState(EnemyAIStateManager enemy)
    {
        //if (enemy.thisUnit.hasMoved)
    }


}
