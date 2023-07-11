using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle : State
{
    EnemyUnitScript enemyUnitScript;
    public ChasePlayerState chasePlayerState;
    public ChaseDefenseState chaseDefenseState;
    public Transform target1;
    public Transform target2;


    void Start()
    {
       
       
    }


    void Update()
    {
        target1 = FindObjectOfType<PlayerUnitScript>().transform;
        target2 = FindObjectOfType<DefenceStructure>().transform;
    }
    
    public override State RunCurrentState()
    {
        if (Vector2.Distance(transform.position, target1.position) < Vector2.Distance(transform.position, target2.position))
        {
            return chasePlayerState;
        }
        else if (Vector2.Distance(transform.position, target2.position) < Vector2.Distance(transform.position, target1.position))
        {
            return chaseDefenseState;
        }
        else
        {
            return this;
        }
    }
}
        
    

