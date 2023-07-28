using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIsMovingState : EnemyAIBaseScript
{
    public override void EnterState(EnemyAIStateManager enemy)
    {
        Debug.Log("Enemy is moving");
        
    }

    public override void UpdateState(EnemyAIStateManager enemy)
    {
        //add enemy movement code here
        // yes the code NEEDS to be in the update function.
        //after that mark self as "hasMoved = true", go back  to idle state
    }
}
