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
     * 
     */
    private EnemyAIStateManager selectedEnemy;

    private void Awake()
    {
        GameEventSystem.current.onEnemyTurnStart += OnEnemyTurnStart;
        GameEventSystem.current.onEnemyTurnEnd += cleanUpEnemyList;
    }

    private void OnDestroy()
    {
        GameEventSystem.current.onEnemyTurnStart -= OnEnemyTurnStart;
        GameEventSystem.current.onEnemyTurnEnd -= cleanUpEnemyList;
    }


    private void OnEnemyTurnStart()
    {
        StartCoroutine(ExecuteAttacksThenMovement());
    }
    private IEnumerator ExecuteAttacksThenMovement()
    {
        List<EnemyAIStateManager> attackingEnemies = new List<EnemyAIStateManager>();

        //Execute attacks for all enemies with attackPrimed == true
        for (int i = 0; i < GameEventSystem.current.enemyUnits.Count; i++)
        {
            if (GameEventSystem.current.enemyUnits[i] != null)
            {
                selectedEnemy = GameEventSystem.current.enemyUnits[i].GetComponent<EnemyAIStateManager>();
                if (selectedEnemy.thisUnit.attackPrimed)
                {
                    attackingEnemies.Add(selectedEnemy);
                }
            }
        }

        //Execute attacks for all the enemies collected in the list
        foreach (EnemyAIStateManager enemy in attackingEnemies)
        {
            enemy.SwitchState(enemy.executeAttackState);
            while (!enemy.thisUnit.hasAttacked)
            {
                yield return null;
            }
        }

        StartCoroutine(ExecuteMovementSequentially());
    }

    private IEnumerator ExecuteMovementSequentially()
    {
        for (int i = 0; i < GameEventSystem.current.enemyUnits.Count; i++)
        {
            if (GameEventSystem.current.enemyUnits[i] != null)
            {
                selectedEnemy = GameEventSystem.current.enemyUnits[i].GetComponent<EnemyAIStateManager>();
                    selectedEnemy.SwitchState(selectedEnemy.calculate);
                    while (!selectedEnemy.thisUnit.turnOver)
                    {
                        yield return null;
                    }
                
            }
        }
    }

    private void cleanUpEnemyList()
    {
        foreach(GameObject enemy in GameEventSystem.current.enemyUnitsToRemove)
        {
            GameEventSystem.current.enemyUnits.Remove(enemy);
        }

        GameEventSystem.current.enemyUnitsToRemove.Clear();
    }
}
