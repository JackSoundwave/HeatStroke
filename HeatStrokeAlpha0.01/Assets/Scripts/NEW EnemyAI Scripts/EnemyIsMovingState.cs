using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class EnemyIsMovingState : EnemyAIBaseScript
{
    public override void EnterState(EnemyAIStateManager enemy)
    {
        Debug.Log("Enemy is moving");
        enemy.movement.FindPathToPlayer();
    }

    public override void UpdateState(EnemyAIStateManager enemy)
    {
        //add enemy movement code here
        // yes the code NEEDS to be in the update function.
        //after that mark self as "hasMoved = true", go back  to idle state
        
        if (!enemy.thisUnit.hasMoved && enemy.movement.path.Count > 0)
        {
            enemy.movement.MoveAlongPath();
        }
        else if (enemy.thisUnit.hasMoved == true && enemy.movement.path.Count <= 0)
        {
            Debug.Log("Turn Over");
            enemy.thisUnit.turnOver = true;
            enemy.SwitchState(enemy.idleState);
            //CombatStateManager.CSInstance.UpdateCombatState(CombatState.PlayerTurn);
        }
    }
}
