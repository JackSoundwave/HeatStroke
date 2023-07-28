using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAIStateManager : MonoBehaviour
{
    EnemyAIBaseScript currentState;
    public EnemyUnitScript thisUnit;
    public EnemyAIidleState idleState = new EnemyAIidleState();
    public EnemyIsMovingState isMovingState = new EnemyIsMovingState();

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
