using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCalculateTargetState : EnemyAIBaseScript
{
    
    public override void EnterState(EnemyAIStateManager enemy)
    {
        Debug.Log("Enemy is calculating optimal move");
    }

    public override void UpdateState(EnemyAIStateManager enemy)
    {
        
    }


}
