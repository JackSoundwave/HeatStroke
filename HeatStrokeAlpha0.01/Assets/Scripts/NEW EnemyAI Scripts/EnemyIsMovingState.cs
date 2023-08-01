using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIsMovingState : EnemyAIBaseScript
{
    private HideAndShowScript targetTile;
    private bool prioritizeStructs;
    private bool prioritizeUnits;
    public override void EnterState(EnemyAIStateManager enemy)
    {
        enemy.movement.inRangeTiles = enemy.movement.rangeFinder.GetTilesInRange(enemy.thisUnit.activeTile, enemy.thisUnit.movementRange);
        prioritizeStructs = enemy.GetPrioritizeStructs();
        prioritizeUnits = enemy.GetPrioritizeUnits();
        //Debug.Log("Entering move state");
        targetTile = enemy.calculate.getBestTile();
        // Debug.Log(targetTile);
        if (targetTile != null)
        {
            enemy.movement.FindPathToTargetTile(targetTile);
            enemy.thisUnit.isMoving = true;

        }
        else
        {
            int index = Random.Range(0, 2);
            if (prioritizeStructs == true)
            {
                enemy.movement.getAllStructures();
                enemy.movement.FindPathToRandomStructure();
                enemy.thisUnit.isMoving = true;
            }
            else if (prioritizeUnits == true) 
            {
                enemy.movement.getAllPlayers();
                enemy.movement.FindPathToRandomPlayer();
                enemy.thisUnit.isMoving = true;
            }
            else
            {
                if (index == 0)
                {
                    enemy.movement.getAllPlayers();
                    enemy.movement.FindPathToRandomPlayer();
                    enemy.thisUnit.isMoving = true;
                }
                else if (index == 1)
                {
                    enemy.movement.getAllStructures();
                    enemy.movement.FindPathToRandomStructure();
                    enemy.thisUnit.isMoving = true;
                }
            }
        }
    }

    public override void UpdateState(EnemyAIStateManager enemy)
    {
        //add enemy movement code here
        // yes the code NEEDS to be in the update function.
        //after that mark self as "hasMoved = true", go back  to idle state
        //enemy.movement.inRangeTiles = enemy.movement.rangeFinder.GetTilesInRange(enemy.thisUnit.activeTile, enemy.thisUnit.movementRange);

        if (CombatStateManager.CSInstance.State == CombatState.EnemyTurn && enemy.thisUnit.turnOver == false)
        {
            if (enemy.thisUnit.isMoving && enemy.movement.path.Count > 0)
            {

                enemy.movement.MoveAlongPath();
            }
            else if (enemy.thisUnit.isMoving == true && enemy.movement.path.Count <= 0)
            {

                enemy.thisUnit.isMoving = false;
                enemy.thisUnit.hasMoved = true;
                enemy.SwitchState(enemy.calculate);
                //CombatStateManager.CSInstance.UpdateCombatState(CombatState.PlayerTurn);
            }
        }
    }
}
