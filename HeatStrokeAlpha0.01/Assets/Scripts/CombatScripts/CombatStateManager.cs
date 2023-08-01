using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class CombatStateManager : MonoBehaviour
{
    //GameState manager, but strictly for combat, so it's a turn-based script.
    public static CombatStateManager CSInstance;

    public CombatState State;
    public bool enemyTurnOver;
    public bool createSpawnTiles;

    //This is when the combatstate in the scene changes, which is what the OnCombatStateChanged variable holds
    public static event Action<CombatState> OnCombatStateChanged;

    private void Awake()
    {
        CSInstance = this;
        //CSInstance.State = CombatState.DeployPhase;
    }

    private void Start()
    {
        UpdateCombatState(CombatState.DeployPhase);
    }

    public void UpdateCombatState(CombatState newState)
    {
        State = newState;

        switch (newState)
        {
            case CombatState.DeployPhase:
                HandleDeployPhase();
                break;
            case CombatState.PlayerTurn:
                HandlePlayerTurn();
                break;
            case CombatState.Decide:
                HandleDecideState();
                break;
            case CombatState.EnemyTurn:
                HandleEnemyTurn();
                break;
            case CombatState.Victory:
                Debug.Log("Victory!");
                break;
            case CombatState.Lose:
                break;
            case CombatState.OutOfCombat:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }

        OnCombatStateChanged?.Invoke(newState);
    }
    
    public void HandleDeployPhase()
    {
        //pretend stuff happens here
    }
    public void HandlePlayerTurn()
    {
        GameEventSystem.current.playerTurnStarted();
        //Debug.Log("Player turn started");        
    }

    //remove async tag later in development, it's here for testing purposes to test the "end turn" button.
    public void HandleEnemyTurn()
    {
        enemyTurnOver = false;
        Debug.Log("Enemy Turn Started");
        GameEventSystem.current.spawnEnemies();
        GameEventSystem.current.enemyTurnStart();
        StartCoroutine(continueTurn());
    }

    public IEnumerator continueTurn()
    {
        while(enemyTurnOver != true)
        {
            yield return null;
        }

        Debug.Log("Enemy Turn Completed");
        GameEventSystem.current.createSpawnTiles();
        GameEventSystem.current.enemyTurnEnd();
        ObjectiveManager.OMInstance.evaluateWinCondition();
    }

    public IEnumerator continueTurn2()
    {
        while(createSpawnTiles != true)
        {
            yield return null;
        }

        Debug.Log("Creating spawn tiles");
        GameEventSystem.current.createSpawnTiles();
    }

    public void HandleDecideState()
    {
        if(HeatGaugeSystem.instance.currentTemperature == HeatGaugeSystem.instance.maxTemperature)
        {
            UpdateCombatState(CombatState.Lose);
        }
        else
        {
            
        }
    }
}

/*
 * Context:
 * Deploy phase = Deployment part, similar to Into The Breach
 * PlayerTurn, EnemyTurn, Victory, Lose = self explanatory.
 * Decide = Runs checks to see if the game should keep going (i.e if HeatGauge == max, immediately trigger lose state)
 */

public enum CombatState
{
    DeployPhase,
    PlayerTurn,
    EnemyTurn,
    Decide,
    Victory,
    Lose,
    OutOfCombat
}
