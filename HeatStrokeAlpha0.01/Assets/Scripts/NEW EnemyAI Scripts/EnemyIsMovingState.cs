using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class EnemyIsMovingState : EnemyAIBaseScript
{
    private HideAndShowScript targetTile;
    public override void EnterState(EnemyAIStateManager enemy)
    {
        Debug.Log("Entering move state");
        targetTile = enemy.calculate.getBestTile();
        enemy.movement.inRangeTiles = enemy.movement.rangeFinder.GetTilesInRange(enemy.thisUnit.activeTile, enemy.thisUnit.movementRange);
        enemy.movement.FindPathToTargetTile(targetTile);
        enemy.thisUnit.isMoving = true;
    }

    public override void UpdateState(EnemyAIStateManager enemy)
    {
        //add enemy movement code here
        // yes the code NEEDS to be in the update function.
        //after that mark self as "hasMoved = true", go back  to idle state
        //enemy.movement.inRangeTiles = enemy.movement.rangeFinder.GetTilesInRange(enemy.thisUnit.activeTile, enemy.thisUnit.movementRange);

        if (CombatStateManager.CSInstance.State == CombatState.EnemyTurn && enemy.thisUnit.turnOver == false)
        {
            /*if (!enemy.thisUnit.isMoving)
            {
                //Debug.Log("Unit not moving, finding path");
                enemy.movement.FindPathToPlayer();
                //Debug.Log("Path value: " + path);
            }*/
            if (enemy.thisUnit.isMoving && enemy.movement.path.Count > 0)
            {
                //Debug.Log("Moving along path");
                enemy.movement.MoveAlongPath();
            }
            else if (enemy.thisUnit.isMoving == true && enemy.movement.path.Count <= 0)
            {
                //Debug.Log("Turn Over");
                enemy.thisUnit.turnOver = true;
                enemy.thisUnit.isMoving = false;

                enemy.attack.victim_P = enemy.attack.findPlayerInAttackRange(enemy.attack.inRangeTiles);
                //enemy.attack.getAttackRange();
                if(enemy.attack.victim_P != null)
                {
                    enemy.attack.primeAttackOnTile(enemy.attack.victim_P.activeTile);
                }
                //enemy.SwitchState(AttackPrimed)
                //CombatStateManager.CSInstance.UpdateCombatState(CombatState.PlayerTurn);
            }
        }
    }
}
