using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State
{
    EnemyUnitScript enemyUnitScript;
    public Idle idle;
    PlayerUnitScript playerUnitScript;

    public override State RunCurrentState()
    {
        Attacking();
        return idle;
    }
    void Attacking()
    {
        
    }
}
    