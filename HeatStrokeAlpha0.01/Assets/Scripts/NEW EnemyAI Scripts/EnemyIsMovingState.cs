using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class EnemyIsMovingState : EnemyAIBaseScript
{
    public override void EnterState(EnemyAIStateManager enemy)
    {
        enemy.movement.inRangeTiles = enemy.movement.rangeFinder.GetTilesInRange(enemy.thisUnit.activeTile, enemy.thisUnit.movementRange);
        enemy.movement.FindPathToPlayer();
    }

    public override void UpdateState(EnemyAIStateManager enemy)
    {
        //add enemy movement code here
        // yes the code NEEDS to be in the update function.
        //after that mark self as "hasMoved = true", go back  to idle state
        enemy.movement.inRangeTiles = enemy.movement.rangeFinder.GetTilesInRange(enemy.thisUnit.activeTile, enemy.thisUnit.movementRange);

        if (CombatStateManager.CSInstance.State == CombatState.EnemyTurn && enemy.thisUnit.turnOver == false)
        {
            if (!enemy.thisUnit.isMoving)
            {
                Debug.Log("Unit not moving, finding path");
                enemy.movement.FindPathToPlayer();
                //Debug.Log("Path value: " + path);
            }
            else if (enemy.thisUnit.isMoving && enemy.movement.path.Count > 0)
            {
                Debug.Log("Moving along path");
                enemy.movement.MoveAlongPath();
            }
            else if (enemy.thisUnit.isMoving == true && enemy.movement.path.Count <= 0)
            {
                Debug.Log("Turn Over");
                enemy.thisUnit.turnOver = true;
                enemy.thisUnit.isMoving = false;
                //CombatStateManager.CSInstance.UpdateCombatState(CombatState.PlayerTurn);
            }
        }
    }
}
