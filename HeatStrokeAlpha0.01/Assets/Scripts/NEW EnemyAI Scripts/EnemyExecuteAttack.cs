using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyExecuteAttack : EnemyAIBaseScript
{
    private HideAndShowScript targetTile;
    public override void EnterState(EnemyAIStateManager enemy)
    {
        targetTile = enemy.calculate.getBestTile();

        if(targetTile != null)
        {
            enemy.attack.executeAttackOnTile(targetTile);
            enemy.thisUnit.hasAttacked = true;
            enemy.thisUnit.attackPrimed = false;
            enemy.SwitchState(enemy.idleState);
        }
        else
        {
            enemy.SwitchState(enemy.idleState);
        }
    }
    public override void UpdateState(EnemyAIStateManager enemy)
    {

    }
}
