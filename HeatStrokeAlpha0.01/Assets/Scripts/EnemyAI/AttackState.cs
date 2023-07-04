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
        if (Input.GetKeyDown("space"))
        {
            return idle;
        }
        else
        {
            return this;
        }
    }
    void Attacking()
    {
        
    }
}
    