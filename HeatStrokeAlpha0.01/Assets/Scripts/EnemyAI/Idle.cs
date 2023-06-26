using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle : State
{
    EnemyUnitScript enemyUnitScript;
    BooleanHolder booleanHolder;
    public ChaseState chaseState;
    //public bool canSeeThePlayer;
    public float AttackRadius;
    public Transform target;
   

    void Start()
    {

    }

    void Update()
    {
        if (Vector2.Distance(transform.position, target.position) < AttackRadius)
        {
            booleanHolder.canSeeThePlayer = true;
        }
    }

    public override State RunCurrentState()
    {
        if (booleanHolder.canSeeThePlayer)
        {
            return chaseState;
        }
        else
        {
            return this;
        }
    }
}
        
