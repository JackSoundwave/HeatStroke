using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackPrimedState : EnemyAIBaseScript
{
    private HideAndShowScript targetTile;
    public override void EnterState(EnemyAIStateManager enemy)
    {
        Debug.Log("Enemy is priming attack on tile");
        targetTile = enemy.calculate.getBestTile();
        if (targetTile != null) 
        {
            enemy.attack.primeAttackOnTile(targetTile);
            enemy.thisUnit.attackPrimed = true;
            enemy.thisUnit.turnOver = true;
        }
        else
        {
            enemy.thisUnit.turnOver = true;
        }
    }

    public override void UpdateState(EnemyAIStateManager enemy)
    {

    }
}
