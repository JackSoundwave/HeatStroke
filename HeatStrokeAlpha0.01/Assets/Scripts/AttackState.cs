using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State
{
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
    