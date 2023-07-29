using System.Collections;
using System.Collections.Generic;
using System.IO;
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
        if (CombatStateManager.CSInstance.State == CombatState.EnemyTurn && enemy.thisUnit.turnOver == false)
        {
            if (!enemy.thisUnit.isMoving)
            {
                enemy.movement.FindPathToPlayer();
                //Debug.Log("Path value: " + path);
            }

            if (enemy.thisUnit.isMoving && enemy.movement.path.Count > 0)
            {
                enemy.movement.MoveAlongPath();
            }
            else if (enemy.thisUnit.isMoving == true && enemy.movement.path.Count <= 0)
            {
                Debug.Log("Turn Over");
                enemy.thisUnit.isMoving = true;
                //CombatStateManager.CSInstance.UpdateCombatState(CombatState.PlayerTurn);
            }
        }
    }


}
