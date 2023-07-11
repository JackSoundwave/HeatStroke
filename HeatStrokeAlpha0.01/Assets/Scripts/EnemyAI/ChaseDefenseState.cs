using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseDefenseState : State
{
    EnemyUnitScript enemyUnitScript;
    public Idle idle;

    public override State RunCurrentState()
    {
     
        Communicator.Instance.AttackingDefense = true;
        Invoke("AttackingDefenseFalse", 10);
        return idle;
    }

    private void AttackingDefenseFalse()
    {
        Communicator.Instance.AttackingDefense = false;
    }
}
