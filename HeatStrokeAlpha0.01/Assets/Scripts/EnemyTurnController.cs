using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurnController : MonoBehaviour
{
    /*
     * This script is responsible for handling the entirety of the enemy turn, 
     * given that each enemy has a specific function associated with their stateMachine, 
     * they need to execute these functions appropriately.
     * 
     * We need to pass the list from the GameEventSystem into here. We can also use that list to create things like the UI element to display the AttackOrder.
     * Yippee
     */

    private void Start()
    {
        
    }

    void Update()
    {
        
    }

    void OnEnemyTurnStart()
    {
        foreach(GameObject enemy in GameEventSystem.current.enemyUnits)
        {
            
        }
    }
}
