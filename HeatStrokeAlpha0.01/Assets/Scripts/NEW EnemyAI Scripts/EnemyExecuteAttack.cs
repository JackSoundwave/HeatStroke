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
            enemy.SwitchState(enemy.idleState);
        }
    }
    public override void UpdateState(EnemyAIStateManager enemy)
    {

    }
}
