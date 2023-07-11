using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasePlayerState : State
{
    EnemyUnitScript enemyUnitScript;
    public Idle idle;
    public bool turnOver;

    public override State RunCurrentState()
    {
        Communicator.Instance.AttackingPlayer = true;
        Invoke("AttackingPlayerFalse", 10);
        if (CombatStateManager.CSInstance.State == CombatState.EnemyTurn && turnOver == false)
        {
            return idle;
        }
        else
        {
            return this;
        }
    }
    private void AttackingPlayerFalse()
    {
        Communicator.Instance.AttackingPlayer = false;
    }
}
