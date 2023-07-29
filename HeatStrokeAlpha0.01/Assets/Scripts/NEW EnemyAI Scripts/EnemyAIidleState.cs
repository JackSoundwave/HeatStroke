using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class EnemyAIidleState : EnemyAIBaseScript
{
    
    public override void EnterState(EnemyAIStateManager enemy)
    {
        Debug.Log("Enemy is idle");
    }
    public override void UpdateState(EnemyAIStateManager enemy)
    {
        if (!enemy.thisUnit.turnOver)
        {
            if (enemy.thisUnit.attackPrimed && !enemy.thisUnit.hasAttacked)
            {
                Debug.Log("Enemy executing attack");
                //executeAttack()
                //attackPrimed = false
                enemy.SwitchState(enemy.idleState);
            }
            else if (!enemy.thisUnit.hasMoved)
            {
                enemy.SwitchState(enemy.isMovingState);
            }
        }
    }


}
