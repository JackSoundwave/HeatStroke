using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class EnemyIsMovingState : EnemyAIBaseScript
{
    private List<HideAndShowScript> path;
    public override void EnterState(EnemyAIStateManager enemy)
    {
        Debug.Log("Enemy is moving");
        
    }

    public override void UpdateState(EnemyAIStateManager enemy)
    {
        //add enemy movement code here
        // yes the code NEEDS to be in the update function.
        //after that mark self as "hasMoved = true", go back  to idle state
        /*
        if (hasMoved && path.Count > 0)
        {
            MoveAlongPath();
        }
        else if (enemy.thisUnit.hasMoved == true && path.Count <= 0)
        {
            Debug.Log("Turn Over");
            enemy.thisUnit.turnOver = true;
            //CombatStateManager.CSInstance.UpdateCombatState(CombatState.PlayerTurn);
        }*/
    }
}
