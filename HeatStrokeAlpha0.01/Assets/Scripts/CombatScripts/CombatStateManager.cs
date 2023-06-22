using System;
using System.Collections;
using System.Collections.Generic;
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
    }

    private void Start()
    {
        UpdateCombatState(CombatState.PlayerTurn);
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
            case CombatState.EnemyTurn:
                HandleEnemyTurn();
                break;
            case CombatState.Victory:
                break;
            case CombatState.Lose:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }

        OnCombatStateChanged?.Invoke(newState);
    }
    
    private void HandleDeployPhase()
    {
        //pretend stuff happens here
        CSInstance.UpdateCombatState(CombatState.PlayerTurn);
    }
    private void HandlePlayerTurn()
    {
        Debug.Log("Player turn started");
        Debug.Log("State value:");
        
    }

    private void HandleEnemyTurn()
    {
        Debug.Log("Enemy Turn Started");
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
    Lose
}
