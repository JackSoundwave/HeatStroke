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

    //This is when the combatstate in the scene changes, which is what the OnCombatStateChanged variable holds
    public static event Action<CombatState> OnCombatStateChanged;

    private void Awake()
    {
        CSInstance = this;
        CSInstance.State = CombatState.DeployPhase;
    }

    private void Start()
    {
        //first turn is playerTurn, truthfully it should be the deploy phase, but for now we'll go with PlayerTurn instead.
        //UpdateCombatState(CombatState.PlayerTurn);
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
        Debug.Log("Player turn started");        
    }

    //remove async tag later in development, it's here for testing purposes to test the "end turn" button.
    public async void HandleEnemyTurn()
    {
        Debug.Log("Enemy Turn Started");
        // Perform any initialization or setup here
        GameEventSystem.current.enemyTurnStart();
        // Wait for a short delay before starting the enemy AI's actions
        await Task.Delay(500);
        

        Debug.Log("Enemy Turn Completed");
        GameEventSystem.current.playerTurnStarted();
        CSInstance.UpdateCombatState(CombatState.PlayerTurn);
        // Trigger events or update the game state accordingly
        //GameEventSystem.current.playerTurnStarted();
        //CombatStateManager.CSInstance.UpdateCombatState(CombatState.PlayerTurn);
    }

    public void HandleDecideState()
    {

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
